import React from 'react';
import { hasStringValue, stateFetch } from './utils.js';
import ToolBar from './toolBar.js';

var filterType = {
    text: 'text'
};

/**
 * Displays an interactive table with data from REST
 * Props:
 * url
 *   string, required
 *   Url appended after baseUrl, that is used for fetching data for the table.
 * columns
 *   array<Column>, required
 * filters
 *   array<Filter>, optional
 * rowButtons
 *   array<RowButton>, optional
 * buttons
 *   array<Button>, optional
 */
class TableManager extends React.Component {
    /**
     * @typedef {Object} Column
     * @property {string} title
     * @property {string} propertyName
     * @property {boolean} sortable Default = true
     * @property {Function} onCellRender (rowData) => cellJsx
     * @property {number} buttonColumnIndex
     *
     * @typedef {Object} Filter
     * @property {string} label
     * @property {string} propertyName
     * @property {string} type Default = 'text'
     * 
     * @typedef {Object} RowButton
     * @property {string} icon
     * @property {string} tooltip
     * @property {Function} onClick
     * @property {string} buttonClass Default = 'btn-light'
     * 
     * @typedef {Object} Button Same as ToolBar Button
     * @property {string} text
     * @property {Object} onClick
     * @property {string} href
     * @property {string} className
     */

    constructor(props) {
        super(props);

        this.state = {
            loadingRows: false,
            error: false,
            results: {
                rows: [],
                totalRows: 0,
                totalPages: 1
            },
            filters: {},
            currentPage: 0,
            pageSize: 10
        };

        this.pageSizeOptions = [5, 10, 20, 100, 500];
        this.fetchRowsDelay = 500;//ms
        this.fetchTimeOutId = null;
    }

    componentDidMount() {
        this.fetchRows();
    }

    /**
     * @returns {Array<Column>}
     */
    getColumns() {
        var columns = [];
        if (Array.isArray(this.props.columns)) {
            for (var i = 0; i < this.props.columns.length; i++) {
                var column = this.props.columns[i];
                if (!hasStringValue(column.propertyName)) {
                    this.logError('Column ' + i + ' is missing propertyName');
                    column.propertyName = 'column' + i;
                }
                column.title = hasStringValue(column.title) ? column.title : column.propertyName;
                column.sortable = column.sortable !== false;
                column.buttonColumnIndex = -1;
                columns.push(column);
            }
        }
        for (var i = 0; i < this.getRowButtons().length; i++) {
            columns.push({
                title: '',
                propertyName: '',
                sortable: false,
                onCellRender: null,
                buttonColumnIndex: i
            });
        }
        return columns;
    }

    /**
     * @returns {Array<Filter>}
     */
    getFilters() {
        var filters = [];
        if (Array.isArray(this.props.filters)) {
            for (var i = 0; i < this.props.filters.length; i++) {
                var filter = this.props.filters[i];
                if (!hasStringValue(filter.propertyName)) {
                    this.logError('Filter ' + i + ' is missing propertyName');
                    filter.propertyName = 'filter' + i;
                }
                filter.label = hasStringValue(filter.label) ? filter.label : filter.propertyName;
                filter.type = hasStringValue(filter.type) ? filter.type : filterType.text;
                filters.push(filter);
            }
        }
        return filters;
    }

    /**
     * @returns {Array<RowButton>}
     */
    getRowButtons() {
        var buttons = [];
        if (Array.isArray(this.props.rowButtons)) {
            for (var i = 0; i < this.props.rowButtons.length; i++) {
                var button = this.props.rowButtons[i];
                if (!hasStringValue(button.icon)) {
                    this.logError('Row button ' + i + ' is missing icon name');
                    button.icon = 'fa-solid fa-question';
                }
                buttons.push(button);
            }
        }
        return buttons;
    }

    /**
     * @returns {Array<Button>}
     */
    getButtons() {
        var buttons = [];
        if (Array.isArray(this.props.buttons)) {
            for (var i = 0; i < this.props.buttons.length; i++) {
                buttons.push(this.props.buttons[i]);
            }
        }
        return buttons;
    }

    fetchRows() {
        this.clearFetchRowsTimeout();
        stateFetch(this, '/VideoGame/Search', {
            loadingProperty: 'loadingRows',
            errorProperty: 'error',
            resultsProperty: 'results',
            method: 'POST',
            body: {
                currentPage: this.state.currentPage,
                pageSize: this.state.pageSize,
                filters: this.state.filters
            }
        });
    }

    clearFetchRowsTimeout() {
        if (this.fetchTimeOutId !== null) {
            clearTimeout(this.fetchTimeOutId);
            this.fetchTimeOutId = null;
        }
    }

    fetchRowsDelayed() {
        this.clearFetchRowsTimeout();
        this.fetchTimeOutId = setTimeout(() => {
            this.fetchRows();
            this.fetchTimeOutId = null;
        }, this.fetchRowsDelay);
    }

    logError(message) {
        console.log('TableManager: ' + message);
    }

    logFilterTypeNotImplementedError(type) {
        this.logError('Filter type not implemented: "' + type + '"');
    }

    /**
     * @param {Filter} filter
     * @param {string} value
     */
    handleTextFilterChange(filter, value) {
        this.setState((previousState) => ({
            filters: {
                ...previousState.filters,
                [filter.propertyName]: value ?? ''
            }
        }), () => {
            this.fetchRowsDelayed();
        });
    }

    handleFirstPageClick() {
        if (this.state.currentPage !== 0) {
            this.setState({
                currentPage: 0
            }, () => {
                this.fetchRows();
            });
        }
    }

    handlePreviousPageClick() {
        if (this.state.currentPage !== 0) {
            this.setState((previousState) => ({
                currentPage: previousState.currentPage - 1
            }), () => {
                this.fetchRows();
            });
        }
    }

    handleLastPageClick() {
        if (this.state.currentPage < this.state.results.totalPages - 1) {
            this.setState({
                currentPage: this.state.results.totalPages - 1
            }, () => {
                this.fetchRows();
            });
        }
    }

    handleNextPageClick() {
        if (this.state.currentPage < this.state.results.totalPages - 1) {
            this.setState((previousState) => ({
                currentPage: previousState.currentPage + 1
            }), () => {
                this.fetchRows();
            });
        }
    }

    handlePageChange(value) {
        value = parseInt(value);
        if (Number.isNaN(value)) {
            value = 0;
        } else {
            value--;
        }
        this.setState({
            currentPage: value
        }, () => {
            this.fetchRowsDelayed();
        });
    }

    handlePageSizeChange(size) {
        this.setState({
            pageSize: size
        }, () => {
            this.fetchRows();
        });
    }

    render() {
        var columns = this.getColumns();
        var filters = this.getFilters();
        var rowButtons = this.getRowButtons();
        var toolbarButtons = this.getButtons();

        //Render filters
        var filterRow = null;
        if (filters.length !== 0) {
            filterRow = (
                <div className="row" style={{ marginBottom: '5px' }}>
                    {filters.map((filter, index) => {
                        var content = null;
                        switch (filter.type) {
                            case filterType.text:
                                var elementId = 'filter_' + filter.propertyName;
                                content = (
                                    <>
                                        <label htmlFor={elementId} className="form-label">{filter.label}</label>
                                        <input
                                            type="text"
                                            className="form-control"
                                            id={elementId}
                                            value={hasStringValue(this.state.filters[filter.propertyName]) ? this.state.filters[filter.propertyName] : ''}
                                            onChange={(event) => { this.handleTextFilterChange(filter, event.target.value); }}
                                        />
                                    </>
                                );
                                break;
                            default:
                                this.logFilterTypeNotImplementedError(filter.type);
                                break;
                        }
                        return (
                            <div className="col" key={'filter-column-' + index}>
                                {content}
                            </div>
                        );
                    })}
                </div>
            );
        }

        var rows;
        if (this.state.loadingRows) {
            //Loading, show one row with spinner
            rows = [
                <tr key="spinner-row">
                    <td scope="row" colSpan={columns.length}>
                        <div className="d-flex justify-content-center">
                            <div className="spinner-border" role="status">
                                <span className="visually-hidden">{localization.Loading + '...'}</span>
                            </div>
                        </div>
                    </td>
                </tr>
            ];
        } else {
            rows = this.state.results.rows.map((row, index) => {
                var cells = [];
                for (var i = 0; i < columns.length; i++) {
                    var column = columns[i];
                    var cellContent;
                    if (column.buttonColumnIndex === -1) {
                        //Normal cell
                        if (typeof column.onCellRender === 'function') {
                            //Custom render function was provided in props
                            cellContent = column.onCellRender(row);
                        } else {
                            //Display row data
                            cellContent = row[column.propertyName];
                        }
                    } else {
                        //Button column
                        var button = rowButtons[column.buttonColumnIndex];
                        cellContent = (
                            <button
                                type="button"
                                className={'icon-button btn ' + (hasStringValue(button.buttonClass) ? button.buttonClass : 'btn-light')}
                                title={hasStringValue(button.tooltip) ? button.tooltip : undefined}
                                onClick={() => { button.onClick(row); }}
                            >
                                <i className={button.icon}></i>
                            </button>
                        );
                    }
                    cells.push(
                        <td key={'row-' + index + '-cell-' + i} className={column.buttonColumnIndex === -1 ? '' : 'button-column-cell'}>
                            {cellContent}
                        </td>
                    );
                }
                return (
                    <tr key={'row-' + index}>
                        {cells}
                    </tr>
                );
            });
        }

        //Toolbar
        var toolbar = null;
        if (toolbarButtons.length !== 0) {
            toolbar = (
                <ToolBar buttons={toolbarButtons} />
            );
        }

        return (
            <div>
                {this.state.error ? <div className="alert alert-warning" role="alert">{localization.GenericErrorMessage}</div> : null}
                {filterRow}
                <table className="table">
                    <thead>
                        <tr>
                            {columns.map((column, index) => {
                                return (
                                    <th scope="col" key={'column-header-' + index} className={column.buttonColumnIndex === -1 ? '' : 'button-column-header'}>
                                        {column.title}
                                    </th>
                                );
                            })}
                        </tr>
                    </thead>
                    <tbody>
                        {rows}
                    </tbody>
                    <tfoot>
                        <tr>
                            <td colSpan={columns.length}>
                                <div className="pagination-container">
                                    <div className="pagination">
                                        <button type="button" className="icon-button btn btn-primary" onClick={() => { this.handleFirstPageClick(); }}>
                                            <i className="fa-solid fa-angles-left"></i>
                                        </button>
                                        <button type="button" className="icon-button btn btn-primary" onClick={() => { this.handlePreviousPageClick(); }}>
                                            <i className="fa-solid fa-angle-left"></i>
                                        </button>
                                        <input
                                            type="text"
                                            className="form-control"
                                            value={this.state.currentPage + 1}
                                            onChange={(event) => { this.handlePageChange(event.target.value); }}
                                        />
                                        <div className="pagination-divider"><span>/</span></div>
                                        <div className="pagination-total-pages"><span>{this.state.results.totalPages}</span></div>
                                        <button type="button" className="icon-button btn btn-primary" onClick={() => { this.handleNextPageClick(); }}>
                                            <i className="fa-solid fa-angle-right"></i>
                                        </button>
                                        <button type="button" className="icon-button btn btn-primary" onClick={() => { this.handleLastPageClick(); }}>
                                            <i className="fa-solid fa-angles-right"></i>
                                        </button>
                                    </div>
                                    <div className="dropdown">
                                        <button className="btn btn-secondary dropdown-toggle" type="button" data-bs-toggle="dropdown">{localization.RowsPerPage + ' ' + this.state.pageSize}</button>
                                        <ul className="dropdown-menu">
                                            {this.pageSizeOptions.map(option => {
                                                return (
                                                    <li key={'page-size-option-' + option}>
                                                        <a className="dropdown-item" onClick={() => { this.handlePageSizeChange(option); }}>
                                                            {option}
                                                        </a>
                                                    </li>
                                                );
                                            })}
                                        </ul>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </tfoot>
                </table>
                {toolbar}
            </div>
        );
    }
}

export default TableManager;