import React from 'react';
import { hasStringValue, stringCheck } from './utils.js';

/**
 * @typedef {Object} ModalButton
 * @property {string} text
 * @property {Function} onClick
 * @property {string} className Default = 'btn-primary'
 * @property {boolean} disabled
 * 
 * @param {Object} props
 * @param {boolean} props.open Optional, default = false
 * @param {string} props.title Optional, default = ''
 * @param {string} props.closeButtonText Optional, default = localization.Close. If set to null or '', close button will be hidden.
 * @param {Function} props.onCloseClick Optional
 * @param {string} props.okButtonText Optional, default = localization.Ok
 * @param {Function} props.onOkClick Optional. Ok button will not be shown, if this is not provided
 * @param {Array<ModalButton>} props.buttons Optional
 * @returns {JSX.Element}
 */
export default function Modal(props) {
    //Event handlers
    function handleCloseClick() {
        if (typeof props.onCloseClick === 'function') {
            props.onCloseClick();
        }
    }

    //Helper functions
    /**
     * @returns {Array<ModalButton>}
     */
    function getButtons() {
        /** @type {Array<ModalButton>} */
        var buttons = [];

        if (Array.isArray(props.buttons)) {
            for (var i = 0; i < props.buttons.length; i++) {
                buttons.push({
                    ...props.buttons[i],
                    className: hasStringValue(props.buttons[i].className) ? props.buttons[i].className : 'btn-primary'
                });
            }
        }

        if (props.closeButtonText !== null && props.closeButtonText !== '') {
            //Add default close button
            buttons.push({
                text: props.closeButtonText === undefined ? localization.Close : props.closeButtonText,
                onClick: () => { handleCloseClick(); },
                className: 'btn-secondary',
                disabled: false
            });
        }

        if (typeof props.onOkClick === 'function') {
            //Add default ok button
            buttons.push({
                text: hasStringValue(props.okButtonText) ? props.okButtonText : localization.Ok,
                onClick: props.onOkClick,
                className: 'btn-primary',
                disabled: false
            });
        }

        return buttons;
    }

    //TODO: Add some fancy slide/fade animations
    var open = props.open === true;
    return (
        <div
            className={'modal fade' + (open ? ' show' : '')}
            tabIndex="-1"
            style={{ display: open ? 'block' : 'none' }}
            aria-hidden={open ? undefined : 'true'}
            aria-modal={open ? 'true' : undefined}
            role={open ? 'dialog' : undefined}
        >
            <div className="modal-dialog" style={{ zIndex: 1 }}>
                <div className="modal-content">
                    <div className="modal-header">
                        <h1 className="modal-title fs-5">{stringCheck(props.title)}</h1>
                        <button type="button" className="btn-close" onClick={() => { handleCloseClick(); }} />
                    </div>
                    <div className="modal-body">
                        {props.children}
                    </div>
                    <div className="modal-footer">
                        {
                            getButtons().map((button, index) => {
                                return (
                                    <button type="button" key={index} disabled={button.disabled === true ? true : undefined} className={'btn ' + button.className} onClick={button.onClick}>{button.text}</button>
                                );
                            })
                        }
                    </div>
                </div>
            </div>
            <div className="modal-dimmer" onClick={() => { handleCloseClick(); }}>
            </div>
        </div>
    );
}