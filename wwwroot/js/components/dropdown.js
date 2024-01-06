import React from 'react';
import { hasStringValue, generateElementId } from './utils.js';

/**
 * Props:
 * label
 *   string, optional
 * selected
 *   any, required
 * options
 *   Array<Option>, required
 * required
 *   boolean, default = false
 * marginBottom
 *   number, default = 3
 * error
 *   boolean, default = false
 * errorMessage
 *   string|any, optional
 * onChange
 *   function(newId), optional
 */
class Dropdown extends React.Component {
    /**
     * @typedef {Object} Option
     * @property {any} id
     * @property {string} text
     */

    constructor(props) {
        super(props);

        this.state = {
            elementId: generateElementId('TextInput')
        };
    }

    handleOptionClick(newId) {
        if (typeof this.props.onChange === 'function') {
            this.props.onChange(newId);
        }
    }

    /**
     * @returns {Array<Option>}
     */
    getOptions() {
        return Array.isArray(this.props.options) ? this.props.options : [];
    }

    render() {
        var helpTextId = this.state.elementId + '_help';
        var options = this.getOptions();
        var selected = options.find(option => option.id === this.props.selected);

        if (selected) {
            //Put the selected value first in the array, to show it as selected
            options = options.filter(option => option.id !== selected.id);
            options.splice(0, 0, selected);
            console.log(options);
        }
        console.log(options);
        console.log(this.props.selected);

        return (
            <div className={'mb-' + (this.props.marginBottom ?? 3)}>
                {
                    hasStringValue(this.props.label) ?
                        <label htmlFor={this.state.elementId} className="form-label">
                            {this.props.label + (this.props.required === true ? ' *' : '')}
                        </label>
                        : null
                }
                <select
                    className={'form-select' + (this.props.error === true ? ' dropdown-error' : '')}
                    aria-label={hasStringValue(this.props.label) ? this.props.label : undefined}
                    id={this.state.elementId}
                    aria-describedby={this.props.errorMessage ? helpTextId : undefined}
                >
                    {
                        options.map((option, index) => {
                            return (
                                <option value={option.id} key={index} onClick={() => { this.handleOptionClick(option.id); }}>
                                    {option.text}
                                </option>
                            );
                        })
                    }
                </select>
                {
                    this.props.errorMessage ?
                        <div id={helpTextId} className="form-text input-error-help-message">{this.props.errorMessage}</div>
                        : null
                }
            </div>
        );
    }
}

export default Dropdown;