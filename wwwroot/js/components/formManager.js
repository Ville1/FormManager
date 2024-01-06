import React from 'react';
import { hasStringValue, toInt, stateFetch, emptyGuid } from './utils.js';
import ToolBar from './toolBar.js';
import TextInput from './textInput.js';
import Dropdown from './dropdown.js';
import Toast from './toast.js';

var formState = {
    new: 'new',
    edit: 'edit',
    readOnly: 'readOnly'
};

var inputType = {
    text: 'text',
    dropdown: 'dropdown'
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
     * @property {Array<any>} options
     * 
     * @typedef {Object} FormRow
     * @property {string} title
     * @property {Array<Input>} inputs
     * 
     * @typedef {Object} DataContainer
     * @property {string} property
     * @property {any} value
     * @property {any} oldValue
     * @property {Array<string>} errors
     * @property {boolean} errorGrace True = hide error messages
     */

    constructor(props) {
        super(props);

        this.state = {
            error: false,
            currentState: formState.new,
            loading: false,
            saving: false,
            id: emptyGuid,
            /** @type {Array<DataContainer>} */
            data: [],
            responseData: {}
        };

        //Initialize form data
        this.initializeData();

        //Ref for showing toast messages
        this.toast = React.createRef();
    }

    componentDidMount() {
        //Determine initial form state
        var searchParams = (new URL(document.location)).searchParams;
        if (searchParams.has('id')) {
            //Load a pre-existing form
            this.loadForm(searchParams.get('id'));
        }
    }

    initializeData() {
        var rows = this.getRows();
        var data = [];
        for (var i = 0; i < rows.length; i++) {
            var row = rows[i];
            for (var j = 0; j < row.inputs.length; j++) {
                var input = row.inputs[j];
                var dataContainer = {
                    property: input.property,
                    value: null,
                    oldValue: null,
                    errors: [],
                    errorGrace: false
                };

                switch (input.type) {
                    case inputType.text:
                        dataContainer.value = '';
                        dataContainer.oldValue = '';
                        break;
                    case inputType.dropdown:
                        dataContainer.value = null;
                        dataContainer.oldValue = null;
                        break;
                    default:
                        this.logInputTypeNotImplementedError(input.type);
                        break;
                }

                data.push(dataContainer);
            }
        }

        this.state.data = data;
        this.validate(null, true);
    }

    loadForm(id) {
        stateFetch(this, this.props.url + '?id=' + id, {
            loadingProperty: 'loading',
            errorProperty: 'error',
            resultsProperty: 'responseData',
            callback: (success) => {
                if (success) {
                    //Form loaded to responseData
                    var data = this.copyData();
                    var inputs = this.getInputs();

                    //Loop inputs
                    for (var i = 0; i < inputs.length; i++) {
                        var input = inputs[i];
                        var responseData = this.state.responseData[input.property];
                        var formData = this.getData(input.property, data);
                        formData.errorGrace = false;

                        //Set input value
                        switch (input.type) {
                            case inputType.text:
                                formData.value = responseData;
                                formData.oldValue = responseData;
                                break;
                            case inputType.dropdown:
                                formData.value = responseData;
                                formData.oldValue = responseData;
                                break;
                            default:
                                this.logInputTypeNotImplementedError(input.type);
                                break;
                        }
                    }

                    this.setState({
                        currentState: formState.edit,
                        data: data,
                        id: id
                    }, () => {
                        this.validate();
                    });
                } else {
                    //Failed to load form
                    this.setState({
                        currentState: formState.readOnly
                    });
                }
            }
        });
    }

    /**
     * Get all rows in the form
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
     * Get all inputs in the form
     * @returns {Array<Input>}
     */
    getInputs() {
        var rows = this.getRows();
        var inputs = [];
        for (var i = 0; i < rows.length; i++) {
            var row = rows[i];
            for (var j = 0; j < row.inputs.length; j++) {
                inputs.push(row.inputs[j]);
            }
        }
        return inputs;
    }

    /**
     * @param {string} property
     * @param {Array<DataContainer>} data
     * @returns {DataContainer}
     */
    getData(property, data) {
        return (data ?? this.state.data).find(x => x.property === property);
    }

    /**
     * Get a clone of current this.state.data - array
     * @returns {Array<DataContainer>}
     */
    copyData() {
        return this.state.data.map(x => {
            return { ...x };
        });
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
        var formData = this.getData(input.property);
        var errorMessage = null;
        var showError = formData.errors.length !== 0 && !formData.errorGrace;
        if (showError) {
            errorMessage = [];
            for (var i = 0; i < formData.errors.length; i++) {
                errorMessage.push(formData.errors[i]);
                if (i !== formData.errors.length - 1) {
                    errorMessage.push(<br />);
                }
            }
        }

        var key = 'input-' + rowIndex + '-' + elementIndex;
        switch (input.type) {
            case inputType.text:
                return (
                    <TextInput
                        key={key}
                        label={input.label}
                        value={formData.value}
                        required={input.required}
                        maxLength={input.maxLength}
                        onChange={(value) => { this.handleChange(input, value); }}
                        error={showError}
                        errorMessage={errorMessage}
                    />
                );
            case inputType.dropdown:
                return (
                    <Dropdown
                        key={key}
                        label={input.label}
                        selected={formData.value}
                        required={input.required}
                        onChange={(id) => { this.handleChange(input, id); }}
                        error={showError}
                        errorMessage={errorMessage}
                        options={input.options}
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
        var data = this.copyData();

        switch (input.type) {
            case inputType.text:
                this.getData(input.property, data).value = value;
                break;
            case inputType.dropdown:
                this.getData(input.property, data).value = value;
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

    handleSave() {
        //Parse request body
        var body = {
            id: this.state.id
        };
        for (var i = 0; i < this.state.data.length; i++) {
            var formData = this.state.data[i];
            body[formData.property] = formData.value;
        }

        //Save
        stateFetch(this, this.props.url, {
            method: 'POST',
            loadingProperty: 'saving',
            body: body,
            callback: (success, data) => {
                if (success) {
                    var toastMessage = this.state.currentState === formState.new ? localization.FormSavedToastMessageNew : localization.FormSavedToastMessagePreExisting;
                    toastMessage = toastMessage.replace('[name]', hasStringValue(this.props.title) ? this.props.title.toLowerCase() : '???');
                    this.toast.current.show({
                        title: localization.FormSavedToastTitle,
                        message: toastMessage
                    });
                    this.setState({
                        id: data,
                        currentState: formState.edit
                    });
                } else {
                    //Error
                    if (!data || !data.errors) {
                        //No validation errors, show generic error message
                        this.setState({
                            error: true
                        });
                    } else {
                        //Show validation errors
                        var formData = this.copyData();

                        for (var errorPropertyName in data.errors) {
                            var errorFormData = formData.find(x => x.property === errorPropertyName);
                            if (errorFormData) {
                                errorFormData.errors = data.errors[errorPropertyName].map(errorMessage => { return localization.ValidationError + ': ' + errorMessage; });
                            } else {
                                this.logError('Backend validation returned a reference to an unexistent property: \"' + errorPropertyName + '\"');
                            }
                        }

                        this.setState({
                            data: formData
                        });
                    }
                }
            }
        });
    }

    /**
     * @param {string} property Property name. If not provided, validate all inputs
     * @param {boolean} initialization If true uses uses '=' - operation and sets errorGrace to true, otherwise uses setState-function
     */
    validate(property, initialization) {
        var data = this.copyData();
        var rows = this.getRows();
        var propertyValidated = false;

        //Loop form rows
        for (var i = 0; i < rows.length && !propertyValidated; i++) {
            var row = rows[i];

            //Loop inputs in the row
            for (var j = 0; j < row.inputs.length && !propertyValidated; j++) {
                var input = row.inputs[j];

                if (hasStringValue(property) && input.property !== property) {
                    //property - parameter was used, and this is not the selected input
                    continue;
                }

                //Get form data associated with current input
                var formData = this.getData(input.property, data);

                //Clear errors
                formData.errors = [];

                if (initialization) {
                    formData.errorGrace = true;
                } else {
                    formData.errorGrace = false;
                }

                switch (input.type) {
                    case inputType.text:
                        if (input.required && !hasStringValue(formData.value)) {
                            //Text input is missing data
                            formData.errors.push(localization.MissingRequiredData);
                        }
                        break;
                    case inputType.dropdown:
                        if (input.required && formData.value === null) {
                            //No value selected in dropdown
                            formData.errors.push(localization.MissingRequiredData);
                        }
                        break;
                    default:
                        this.logInputTypeNotImplementedError(input.type);
                        break;
                }

                if (hasStringValue(property) && input.property === property) {
                    //property - parameter was used, and it has now been validated
                    propertyValidated = true;
                }
            }
        }

        if (initialization === true) {
            this.state.data = data;
        } else {
            this.setState({
                data: data
            });
        }
    }

    isReadOnly() {
        return this.state.currentState === formState.readOnly || this.state.loading || this.state.saving;
    }

    canSave() {
        if (this.isReadOnly()) {
            return false;
        }
        for (var i = 0; i < this.state.data.length; i++) {
            if (this.state.data[i].errors.length !== 0) {
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
                        onClick: () => { this.handleSave(); },
                        disabled: !this.canSave()
                    }
                ]} />
                <Toast ref={this.toast} />
            </div>
        );
    }
}

export default FormManager;