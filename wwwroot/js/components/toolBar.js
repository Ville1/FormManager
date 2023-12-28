import React from 'react';
import { hasStringValue } from './utils.js';

class ToolBar extends React.Component {
    /**
     * @typedef {Object} Button
     * @property {string} text
     * @property {Object} onClick
     * @property {string} href
     * @property {string} className
     * @property {boolean} disabled Default = false
     */

    constructor(props) {
        super(props);
    }

    /**
     * @returns {Button[]}
     */
    getButtons() {
        return Array.isArray(this.props.buttons) ? this.props.buttons : []; 
    }

    render() {
        return (
            <nav className="navbar fixed-bottom tool-bar">
                <div className="container-fluid justify-content-start">
                    <form className="d-flex">
                        {this.getButtons().map((button, index) => {
                            return (
                                hasStringValue(button.href) ?
                                    <a
                                        key={index}
                                        className={'btn ' + (hasStringValue(button.className) ? button.className : 'btn-primary')}
                                        href={button.href}
                                        role="button"
                                        disabled={button.disabled === true}
                                    >
                                        {button.text}
                                    </a> :
                                    <button
                                        key={index}
                                        className={'btn ' + (hasStringValue(button.className) ? button.className : 'btn-primary')}
                                        onClick={() => { if (typeof button.onClick === 'function' && button.disabled !== true) { button.onClick(); }; }}
                                        disabled={button.disabled === true}
                                    >
                                        {button.text}
                                    </button>

                            );
                        })}
                    </form>
                </div>
            </nav>
        );
    }
}

export default ToolBar;