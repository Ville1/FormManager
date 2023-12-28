import React from 'react';
import { hasStringValue, randomString } from './utils.js';

/**
 * Props:
 * label
 *   string, optional
 * value
 *   string, required
 * required
 *   boolean, default = false
 * marginBottom
 *   number, default = 3
 * maxLength
 *   number, default = 100000
 */
class TextInput extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            elementId: ''
        };

        for (var i = 0; i < 999999 && this.state.elementId.length === 0; i++) {
            var newId = randomString(20);
            if (document.getElementById(newId) === null) {
                this.state.elementId = newId;
            }
        }
        if (this.state.elementId.length === 0) {
            console.log('TextInput: Failed to find a free element id');
            this.state.elementId = 'TextInput';
        }
    }

    componentDidMount() {

    }

    handleChange(newValue) {
        newValue = hasStringValue(newValue) ? newValue : '';

        if (typeof this.props.onChange === 'function') {
            this.props.onChange(newValue);
        }
    }

    getValue() {
        return hasStringValue(this.props.value) ? this.props.value : '';
    }

    render() {
        return (
            <div className={'mb-' + (this.props.marginBottom ?? 3)}>
                {
                    hasStringValue(this.props.label) ?
                        <label htmlFor={this.state.elementId} className="form-label">
                            {this.props.label + (this.props.required === true ? ' *' : '')}
                        </label>
                        : null
                }
                <input className="form-control" id={this.state.elementId} onChange={(event) => { this.handleChange(event.target.value); }} />
            </div>
        );
    }
}

export default TextInput;