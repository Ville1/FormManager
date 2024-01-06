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
            elementId: generateElementId('Dropdown')
        };

        this.emptyOptionId = '_EMPTY_DROPDOWN_OPTION_';
    }

    handleOptionClick(newId) {
        if (newId !== null && typeof this.props.onChange === 'function') {
            this.props.onChange(newId);
        }
    }

    /**
     * @returns {Array<Option>}
     */
    getOptions() {
        var options = Array.isArray(this.props.options) ? this.props.options.map(x => { return { ...x }; }) : [];
        if (options.find(option => option.id === this.emptyOptionId)) {
            console.log('Dropdown: Option has a disallowed id "' + this.emptyOptionId + '"');
        }
        return options;
    }

    render() {
        var helpTextId = this.state.elementId + '_help';
        var options = this.getOptions().map(x => { return { ...x }; });
        var selected = options.find(option => option.id === this.props.selected);

        if (!selected) {
            //Add empty value as selected
            options.push({
                id: this.emptyOptionId,
                text: ''
            });
        }

        return (
            <div className={'form-input mb-' + (this.props.marginBottom ?? 3)}>
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
                    value={selected !== undefined ? selected.id : this.emptyOptionId}
                >
                    {
                        options.map((option, index) => {
                            return (
                                <option value={option.id} key={index} onClick={() => { this.handleOptionClick(option.id); }} className={option.id === this.emptyOptionId ? 'dropdown-hidden-option' : ''}>
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