export const emptyGuid = '00000000-0000-0000-0000-000000000000';

export function hasStringValue(p) {
    return typeof p === 'string' && p.length !== 0;
}

/**
 * Checks if parameter is string. If it is, parameter is returned, otherwise an empty string is returned.
 */
export function stringCheck(p) {
    return typeof p === 'string' ? p : '';
}

/**
 * @param {any} param
 * @param {number} defaultValue
 * @returns
 */
export function toInt(param, defaultValue) {
    defaultValue = defaultValue ?? 0;

    if (typeof param === 'number') {
        //Already number, round to make sure it's int
        return Math.round(param);
    }

    if (typeof param !== 'string') {
        //Convert to string
        param = param + '';
    }

    //Parse int, and return defaultValue if NaN
    param = parseInt(param);
    return Number.isNaN(param) ? defaultValue : param;
}

/**
 * @param {number} length
 * @returns {string}
 */
export function randomString(length) {
    var chars = '0123456789abcdefghijklmnopqrstuvwzxyABCDEFGHIJKLMNOPQRSTUVWZXY';
    var string = '';
    for (var i = 0; i < length; i++) {
        string += chars[Math.floor(Math.random() * chars.length)];
    }
    return string;
}

/**
 * @param {string} elementName
 * @returns {string}
 */
export function generateElementId(elementName) {
    var id = '';
    for (var i = 0; i < 999999 && id.length === 0; i++) {
        var newId = randomString(20);
        if (document.getElementById(newId) === null) {
            id = newId;
        }
    }
    if (id.length === 0) {
        console.log(elementName + ': Failed to find a free element id');
        id = elementName;
    }

    return id;
}

/**
 * Makes a http request using fetch-function, and updates React component's state accordingly
 * @param {Object} component React component
 * @param {string} url Appended after baseUrl
 * @param {Object} parameters
 * @param {string} parameters.resultsProperty If not provided, response body is not read
 * @param {string} parameters.errorResultsProperty If provided and response status is not in successStatus, response body is stored in this property. Can be same as resultsProperty.
 * @param {string} parameters.loadingProperty
 * @param {string} parameters.errorProperty If provided, this gets set to true in case an error occurs
 * @param {string} parameters.responseErrorMessageProperty Default = 'message'
 * @param {string} parameters.errorStateMessageProperty If this is provided, response status is not in successStatus array and response body has responseErrorMessageProperty, error message gets set to this property in state
 * @param {boolean} parameters.overwriteErrorProperty Default = true. If set to false, error property will not be set from true to false on successful fetch.
 * @param {Array<number>} parameters.successStatus Default = [200]
 * @param {string} parameters.logErrorMessage
 * @param {Function} parameters.callback Parameters: boolean (true if success, false if error), Object|null response data
 * @param {string} parameters.method Can be used as an alternative to init.method
 * @param {Object} parameters.body Can be used as an alternative to init.body and JSON.stringify
 * @param {RequestInit} init Parameters for fetch-function
 */
export function stateFetch(component, url, parameters, init) {
    //Check parameters
    parameters = parameters ?? {};
    parameters.responseErrorMessageProperty = parameters.responseErrorMessageProperty ?? 'message';
    parameters.overwriteErrorProperty = parameters.overwriteErrorProperty !== false;
    parameters.successStatus = Array.isArray(parameters.successStatus) && parameters.successStatus.length !== 0 ? parameters.successStatus : [200];
    init = init ?? {};

    if (hasStringValue(parameters.method)) {
        init.method = parameters.method;
    }

    if (parameters.body) {
        init.body = JSON.stringify(parameters.body);
        init.headers = {
            "Content-Type": "application/json",
        }
    }

    //State before fetch
    var preFetchState = {};

    if (parameters.loadingProperty) {
        //Set loading property to true
        preFetchState[parameters.loadingProperty] = true;
    }
    if (parameters.errorProperty && parameters.overwriteErrorProperty) {
        //Clear error state
        preFetchState[parameters.errorProperty] = false;
    }

    var error = false;
    component.setState(preFetchState, () => {
        fetch(baseUrl + url, init)
            .then(response => {
                if (parameters.successStatus.indexOf(response.status) === -1) {
                    //Error
                    if (parameters.logErrorMessage) {
                        console.log(parameters.logErrorMessage);
                    } else {
                        console.log('Fetch failed: "' + url + '"');
                    }
                    error = true;
                }
                return (
                    response.headers.get("content-type") !== null && response.headers.get("content-type").indexOf('application/json') !== -1 ?
                        response.json() : null
                );
            })
            .then(data => {
                //State after fetch
                var postFetchState = {};

                if (hasStringValue(parameters.resultsProperty) && !error) {
                    //Successfully fetched data from batckend, set results to state
                    postFetchState[parameters.resultsProperty] = data;
                }

                if (parameters.loadingProperty) {
                    //Set loading property to false
                    postFetchState[parameters.loadingProperty] = false;
                }

                if (parameters.errorProperty) {
                    //Set error property
                    postFetchState[parameters.errorProperty] =
                        (!parameters.overwriteErrorProperty && component.state[parameters.errorProperty] === true) ||//Already in error state, and overwriteErrorProperty is set to false
                        error;//Error result in fetch
                }

                if (error && parameters.errorStateMessageProperty && parameters.responseErrorMessageProperty && data && hasStringValue(data[parameters.responseErrorMessageProperty])) {
                    //Set error message
                    postFetchState[parameters.errorStateMessageProperty] = data[parameters.responseErrorMessageProperty];
                }

                if (hasStringValue(parameters.errorResultsProperty) && error) {
                    //Set error results
                    postFetchState[parameters.errorResultsProperty] = data;
                }

                component.setState(postFetchState, () => {
                    if (typeof parameters.callback === 'function') {
                        parameters.callback(!error, data);
                    }
                });
            })
    });
}
