import React from 'react';
import { stringCheck } from './utils.js';

class Toast extends React.Component {
    /**
     * @typedef {Object} Message
     * @property {number} id
     * @property {string} title
     * @property {string} message
     * @property {number} time Seconds. Default = 3, -1 = permanent
     * @property {number} timeoutId
     */

    constructor(props) {
        super(props);

        this.state = {
            /** @type {Array<Message>} */
            messages: [],
            currentId: 0
        };

        this.defaultTime = 3;
    }

    /**
     * @param {number} messageId
     */
    handleCloseClick(messageId) {
        var messages = this.state.messages.map(x => { return { ...x }; });
        var message = messages.find(x => x.id === messageId);
        if (!message) {
            //Already closed
            return;
        }

        //Remove message from list
        messages = messages.filter(x => x.id !== messageId);

        if (message.timeoutId !== null) {
            //Clear timeout
            clearTimeout(message.timeoutId);
        }

        this.setState({
            messages: messages
        });
    }

    /**
     * @param {Message} message
     */
    show(message) {
        //Check parameters
        if (!message) {
            console.log('Toast message is missing');
            return;
        }
        message.title = stringCheck(message.title);
        message.message = stringCheck(message.message);
        //Time should be > 0 or -1
        message.time = typeof message.time === 'number' ? (message.time === -1 ? message.time : (message.time <= 0 ? this.defaultTime : message.time)) : this.defaultTime;
        message.id = this.state.currentId;
        message.timeoutId = null;

        //Add new message to array
        var messages = this.state.messages.map(x => { return { ...x }; });
        messages.push(message);

        if (message.time > 0) {
            //Add timeout
            message.timeoutId = setTimeout(() => {
                this.handleCloseClick(message.id);
            }, message.time * 1000);
        }

        this.setState({
            messages: messages,
            currentId: message.id + 1
        });
    }

    render() {
        if (this.state.messages.length === 0) {
            //No messages
            return null;
        }
        return (
            <div className="toast-outer-container">
                <div className="toast-container position-static">
                    {
                        this.state.messages.map(message => {
                            return (
                                <div className="toast show" role="alert" key={message.id}>
                                    <div className="toast-header">
                                        {message.title !== '' ? <strong className="me-auto">{message.title}</strong> : null}
                                        <button type="button" className="btn-close" onClick={() => { this.handleCloseClick(message.id); }} />
                                    </div>
                                    {message.message !== '' ? <div className="toast-body">{message.message}</div> : null}
                                </div>
                            );
                        })
                    }
                </div>
            </div>
        );
    }
}

export default Toast;