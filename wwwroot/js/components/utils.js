export function hasStringValue(p) {
    return typeof p === 'string' && p.length !== 0;
}

/**
 * Makes a http request using fetch-function, and updates React component's state accordingly
 * @param {Object} component React component
 * @param {string} url Appended after baseUrl
 * @param {Object} parameters
 * @param {string} parameters.resultsProperty If not provided, response body is not read
 * @param {string} parameters.loadingProperty
 * @param {string} parameters.errorProperty
 * @param {string} parameters.responseErrorMessageProperty Default = 'message'
 * @param {string} parameters.errorStateMessageProperty If this is provided, response status is not in successStatus array and response body has responseErrorMessageProperty, error message gets set to this property in state
 * @param {boolean} parameters.overwriteErrorProperty Default = true. If set to false, error property will not be set from true to false on successful fetch.
 * @param {Array<number>} parameters.successStatus Default = [200]
 * @param {string} parameters.logErrorMessage
 * @param {Function} parameters.callback Parameter = boolean (true if success, false if error)
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
    var readBody = hasStringValue(parameters.resultsProperty);

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
                return response.json();
            })
            .then(data => {
                //State after fetch
                var postFetchState = {};

                if (readBody && !error) {
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

                component.setState(postFetchState, () => {
                    if (typeof parameters.callback === 'function') {
                        parameters.callback(!error);
                    }
                });
            })
    });
}
