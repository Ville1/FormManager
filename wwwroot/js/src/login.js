import { createRoot } from 'react-dom/client';
import React, { useEffect } from 'react';
import { useState } from 'react';
import hasStringValue from '../components/utils.js';

function Login() {
    //State
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [loading, setLoading] = useState(false);
    const [loginFailed, setLoginFailed] = useState(false);

    //Event handlers
    function handleEmailChange(event) {
        setEmail(event.target.value);
    }

    function handlePasswordChange(event) {
        setPassword(event.target.value);
    }

    function handleLoginClick() {
        if (canLogin()) {
            setLoading(true);
            setLoginFailed(false);

            fetch(baseUrl + '/Login/Authenticate?email=' + email + '&password=' + password)
                .then(response => {
                    if (response.status === 200) {
                        //Logged in, redirect to front page
                        window.location = baseUrl + '/Main';
                    } else {
                        //Invalid credentials
                        setLoginFailed(true);
                    }
                    setLoading(false);
                });
        }
    }

    //Helper functions
    function canLogin() {
        return hasStringValue(email) && hasStringValue(password) && !loading;
    }

    var midColumnWidthMobile = 10;
    var sideColumnWidthMobile = 1;
    var midColumnWidthDesktop = 6;
    var sideColumnWidthDesktop = 3;

    return (
        <div className="small main-content container">
            <h3 className="text-center">{localization.FormManagerTitle}</h3>
            {loginFailed ? <div className="alert alert-warning" role="alert">{localization.InvalidCredentialsMessage}</div> : null}
            <div className="row">
                <div className={'col-' + sideColumnWidthMobile + ' col-lg-' + sideColumnWidthDesktop}></div>
                <div className={'col-' + midColumnWidthMobile + ' col-lg-' + midColumnWidthDesktop}>
                    <div className="row mb-1">
                        <label htmlFor="input-email" className="col-sm-2 col-form-label">{localization.Email}</label>
                        <div className="col-sm-10">
                            <input
                                id="input-email"
                                className="form-control"
                                value={email}
                                onChange={handleEmailChange}
                                style={{ maxWidth: 'unset' }}
                                disabled={loading}
                            />
                        </div>
                    </div>
                    <div className="row mb-3">
                        <label htmlFor="input-password" className="col-sm-2 col-form-label">{localization.Password}</label>
                        <div className="col-sm-10">
                            <input
                                id="input-password"
                                className="form-control"
                                type="password"
                                value={password}
                                onChange={handlePasswordChange}
                                style={{ maxWidth: 'unset' }}
                                disabled={loading}
                            />
                        </div>
                    </div>
                    <div className="d-grid col-4 mx-auto">
                        <button
                            type="button"
                            className="btn btn-primary"
                            onClick={handleLoginClick}
                            disabled={!canLogin()}
                        >
                            {
                                loading ?
                                    <>
                                        <span className="spinner-border spinner-border-sm" aria-hidden="true"></span>
                                        <span role="status">{localization.Loading + '...'}</span>
                                    </> : localization.Login
                            }
                        </button>
                    </div>
                </div>
                <div className={'col-' + sideColumnWidthMobile + ' col-lg-' + sideColumnWidthDesktop}></div>
            </div>
        </div>
    );
}

createRoot(document.getElementById('root')).render(<Login />);