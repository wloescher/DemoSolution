import { useState } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import Spinner from 'react-bootstrap/Spinner';
import { useNavigate, useParams } from 'react-router-dom';

// AgGrid
import { AgGridReact } from 'ag-grid-react';
import { AllCommunityModule, ModuleRegistry } from 'ag-grid-community';
ModuleRegistry.registerModules([AllCommunityModule]);

// Functions
import { useLoadData } from '../functions';

const WorkItemDataGrid = ({ isLoading, setIsLoading, recordCount, setRecordCount }) => {
    const [rowData, setRowData] = useState([]);
    const [error, setError] = useState();
    const pagination = true;
    const paginationPageSize = 10;
    const paginationPageSizeSelector = [10, 20, 50, 100];
    const autoSizeStrategy = {
        type: 'fitGridWidth'
    };
    const gridOptions = {
        onRowClicked: (event) => {
            navigate({
                pathname: '/workitem/' + event.data.id
            });
        },
    };
    const [filterText, setFilterText] = useState('');
    const navigate = useNavigate();
    const params = useParams();
    const filter = params.filter;

    // ------------------------------------------------------------
    // Column Definitions
    // ------------------------------------------------------------

    const columnDefs = [
        { headerName: 'Title', field: 'title', filter: true },
        { headerName: 'Client', field: 'clientName', filter: true },
        { headerName: 'Type', field: 'type', filter: true },
        { headerName: 'Status', field: 'status', filter: true },
        { headerName: 'Active', field: 'isActive' },
        { headerName: 'Sub-Title', field: 'subTitle', filter: true },
    ];

    // ------------------------------------------------------------
    // Load data
    // ------------------------------------------------------------

    useLoadData('/test/workitem', setIsLoading, setRowData, setError, setRecordCount, filter);

    // ------------------------------------------------------------
    // Presentation Layer
    // ------------------------------------------------------------

    const contents = isLoading
        ? <div className="m-3"><Spinner size="sm" animation="border" role="status" /> Loading...</div>
        : !rowData
            ? <div className="alert alert-warning" role="alert">
                <FontAwesomeIcon icon="fa-solid fa-exclamation-triangle" className="me-2" /> Data not found.
            </div>
            : <>
                <div className="filter-box btn-group">
                    <button className="btn btn-light border">
                        <FontAwesomeIcon icon="fa-solid fa-filter" className="gridFilter" />
                    </button>
                    <input className="form-control form-control-sm border" type="search" placeholder="Filter..." aria-label="Filter" value={filterText} onChange={(e) => setFilterText(e.target.value)} />
                </div>
                <AgGridReact
                    rowData={rowData}
                    columnDefs={columnDefs}
                    autoSizeStrategy={autoSizeStrategy}
                    pagination={pagination}
                    paginationPageSize={paginationPageSize}
                    paginationPageSizeSelector={paginationPageSizeSelector}
                    quickFilterText={filterText}
                    gridOptions={gridOptions} />
            </>

    return (
        contents
    );
}

export default WorkItemDataGrid;