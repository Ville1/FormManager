import React from 'react';
import { hasStringValue } from './utils.js';

class ToolBar extends React.Component {
    /**
     * @typedef {Object} Button
     * @property {string} text
     * @property {Object} onClick
     * @property {string} href
     * @property {string} className
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
                    {this.getButtons().map((button, index) => {
                        return (
                            hasStringValue(button.href) ?
                                <ul className="navbar-nav" key={index}>
                                    <li className={'nav-item' + (hasStringValue(button.className) ? button.className + ' ' : '')}>
                                        <a className="nav-link active" aria-current="page" href={button.href}>
                                            {button.text}
                                        </a>
                                    </li>
                                </ul> :
                                <form className="d-flex" key={index}>
                                    <button className={'btn ' + (hasStringValue(button.className) ? button.className : 'btn-primary')} onClick={button.onClick}>
                                        {button.text}
                                    </button>
                                </form>

                        );
                    })}
                </div>
            </nav>
        );
    }
}

export default ToolBar;