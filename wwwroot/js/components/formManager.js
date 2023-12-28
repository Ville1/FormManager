import React from 'react';
import { hasStringValue, toInt, stateFetch } from './utils.js';
import ToolBar from './toolBar.js';
import TextInput from './textInput.js';

var formState = {
    new: 'new',
    edit: 'edit',
    readonly: 'readonly'
};

var inputType = {
    text: 'text'
};

/**
 * Props:
 * title
 *   string
 * url
 *   string, required
 * rows
 *   Array<FormRow|Input|Array<Input>>
 */
class FormManager extends React.Component {
    /**
     * @typedef {Object} Input
     * @property {string} label
     * @property {string} property
     * @property {string} type Enum: inputType, default = inputType.text
     * @property {number} maxLength Default = 100000 
     * @property {boolean} required Default = false
     * 
     * @typedef {Object} FormRow
     * @property {string} title
     * @property {Array<Input>} inputs
     */

    constructor(props) {
        super(props);

        this.state = {
            error: false,
            currentState: formState.new,
            data: {}
        };

        this.initializeData();
    }

    componentDidMount() {
    }

    initializeData() {
        var rows = this.getRows();
        var data = {};
        for (var i = 0; i < rows.length; i++) {
            var row = rows[i];
            for (var j = 0; j < row.inputs.length; j++) {
                var input = row.inputs[j];
                switch (input.type) {
                    case inputType.text:
                        data[input.property] = {
                            value: '',
                            errors: []
                        };
                        break;
                    default:
                        this.logInputTypeNotImplementedError(input.type);
                        break;
                }
            }
        }

        this.state.data = data;
        this.validate(null, false);
    }

    /**
     * @returns {Array<FormRow>}
     */
    getRows() {
        if (!Array.isArray(this.props.rows)) {
            //No form rows
            return [];
        }
        var rows = [];

        for (var i = 0; i < this.props.rows.length; i++) {
            var propRow = this.props.rows[i];
            var row = {
                title: '',
                inputs: []
            };

            //Check type of this row
            if (Array.isArray(propRow)) {
                //Array<Input>
                row.inputs = propRow.map(x => this.parsePropInput(x));
            } else if (hasStringValue(propRow.property)) {
                //Input
                row.inputs.push(this.parsePropInput(propRow))
            } else {
                //FormRow
                row.title = hasStringValue(propRow.title) ? propRow.title : '';
                if (Array.isArray(propRow.inputs)) {
                    row.inputs = propRow.inputs.map(x => this.parsePropInput(x));
                }
            }

            rows.push(row);
        }

        return rows;
    }

    /**
     * @param {Object} input
     * @returns {Input}
     */
    parsePropInput(input) {
        if (!hasStringValue(input.property)) {
            this.logError('Input is missing property name');
            input.property = '_MISSING_DATA_';
        }

        input.label = hasStringValue(input.label) ? input.label : '[' + input.property + ']';
        input.type = hasStringValue(input.type) ? input.type : inputType.text;
        input.maxLength = toInt(input.maxLength, 100000);

        return input;
    }

    /**
     * @param {FormRow} row
     * @param {number} index
     * @returns {JSX.Element}
     */
    renderRow(row, index) {
        var elements = [];

        for (var i = 0; i < row.inputs.length; i++) {
            elements.push(this.renderInput(row.inputs[i], index, i));
        }

        return (
            <div className="row" key={'row-' + index}>
                {hasStringValue(row.title) ? <div>{row.title}</div> : null}
                {elements}
            </div>
        );
    }

    /**
     * @param {Input} input
     * @param {number} rowIndex
     * @param {number} elementIndex
     * @returns {JSX.Element}
     */
    renderInput(input, rowIndex, elementIndex) {
        switch (input.type) {
            case inputType.text:
                return (
                    <TextInput
                        key={'input-' + rowIndex + '-' + elementIndex}
                        label={input.label}
                        value={this.state.data[input.property].value}
                        required={input.required}
                        maxLength={input.maxLength}
                        onChange={(value) => { this.handleChange(input, value); }}
                    />
                );
            default:
                console.log(this.logInputTypeNotImplementedError(input.type));
                return null;
        }
    }

    /**
     * @param {Input} input
     * @param {Object} value
     */
    handleChange(input, value) {
        var data = { ...this.state.data };

        switch (input.type) {
            case inputType.text:
                data[input.property].value = value;
                break;
            default:
                console.log(this.logInputTypeNotImplementedError(input.type));
                break;
        }

        this.setState({
            data: data
        }, () => {
            this.validate(input.property);
        });
    }

    /**
     * @param {string} property Property name. If not provided, validate all inputs
     * @param {boolean} useSetState If true uses setState-function, otherwise this.state.data = {}
     */
    validate(property, useSetState) {
        var data = { ...this.state.data };
        var rows = this.getRows();
        var propertyValidated = false;

        for (var i = 0; i < rows.length && !propertyValidated; i++) {
            var row = rows[i];
            for (var j = 0; j < row.inputs.length && !propertyValidated; j++) {
                var input = row.inputs[j];
                if (hasStringValue(property) && input.property !== property) {
                    continue;
                }

                switch (input.type) {
                    case inputType.text:
                        data[input.property].errors = [];
                        if (!hasStringValue(data[input.property].value)) {
                            data[input.property].errors.push(localization.MissingRequiredData);
                        }
                        break;
                    default:
                        this.logInputTypeNotImplementedError(input.type);
                        break;
                }

                if (hasStringValue(property) && input.property === property) {
                    propertyValidated = true;
                }
            }
        }

        if (useSetState !== false) {
            this.setState({
                data: data
            });
        } else {
            this.state.data = data;
        }
    }

    canSave() {
        for (const property in this.state.data) {
            if (this.state.data[property].errors.length !== 0) {
                return false;
            }
        }
        return true;
    }

    logError(message) {
        console.log('FormManager: ' + message);
    }

    logInputTypeNotImplementedError(type) {
        this.logError('Input type not implemented: "' + type + '"');
    }

    render() {
        var rows = this.getRows();

        return (
            <div>
                {this.state.error ? <div className="alert alert-warning" role="alert">{localization.GenericErrorMessage}</div> : null}
                <form>
                    {rows.map((row, index) => this.renderRow(row, index))}
                </form>
                <ToolBar buttons={[
                    {
                        text: localization.Save,
                        onClick: () => { console.log('save'); },
                        disabled: !this.canSave()
                    }
                ]}/>
            </div>
        );
    }
}

export default FormManager;