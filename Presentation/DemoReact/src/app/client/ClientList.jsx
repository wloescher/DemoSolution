import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useState } from "react";
import ClientDataGrid from "../client/ClientDataGrid"
import { useLocation, Link } from 'react-router-dom';

function ClientList() {
    const [isClientsLoading, setIsClientsLoading] = useState();
    const [clientsRecordCount, setClientsRecordCount] = useState();
    const [clientsGridRef, setClientsGridRef] = useState();
    const location = useLocation();

    const getButtonClassName = (path) => {
        var className = 'btn btn-outline-secondary';
        if (location.pathname === path) {
            className += ' bg-info';
        }
        return className;
    };

    function onClientsGridReady(params) {
        setClientsGridRef(params.api);
    }

    return (
        <>
            <div className="container">
                <div className="row">
                    <div className="col">
                        <h1>Client List</h1>
                    </div>
                    <div className="col text-end">
                        <div className="btn-group border" role="group" >
                            <Link to="/clients" className={getButtonClassName('/clients')}>ALL</Link>
                            <Link to="/clients/internal" className={getButtonClassName('/clients/internal')}>Internals</Link>
                            <Link to="/clients/external" className={getButtonClassName('/clients/external')}>Externals</Link>
                            <Link to="/clients/lead" className={getButtonClassName('/clients/lead')}>Leads</Link>
                        </div>
                    </div>
                    <div className="col-1 text-end">
                        <Link to="/client/add" className="btn btn-primary">
                            <FontAwesomeIcon icon="fa-solid fa-plus" className="me-2" /> Add
                        </Link>
                    </div>
                </div>
                <div className="data-grid">
                    <ClientDataGrid isLoading={isClientsLoading}
                        setIsLoading={setIsClientsLoading}
                        recordCount={clientsRecordCount}
                        setRecordCount={setClientsRecordCount}
                        onGridReady={onClientsGridReady} />
                </div>
            </div>
        </>

    );
}

export default ClientList;