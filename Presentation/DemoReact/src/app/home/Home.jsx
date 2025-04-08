import { useState } from "react";
import ClientDataGrid from "../client/ClientDataGrid"
import UserDataGrid from "../user/UserDataGrid"
import WorkItemDataGrid from "../workitem/WorkItemDataGrid"

function Home() {
    // Clients
    const [isClientsLoading, setIsClientsLoading] = useState();
    const [clientsRecordCount, setClientsRecordCount] = useState();
    const [clientsGridRef, setClientsGridRef] = useState();

    // Users
    const [isUsersLoading, setIsUsersLoading] = useState();
    const [usersRecordCount, setUsersRecordCount] = useState();
    const [usersGridRef, setUsersGridRef] = useState();

    // WorkItems
    const [isWorkItemsLoading, setIsWorkItemsLoading] = useState();
    const [workItemsRecordCount, setWorkItemsRecordCount] = useState();
    const [workItemsGridRef, setWorkItemsGridRef] = useState();

    function onClientsGridReady(params) {
        setClientsGridRef(params.api);
    }

    function onUsersGridReady(params) {
        setUsersGridRef(params.api);
    }

    function onWorkItemsGridReady(params) {
        setWorkItemsGridRef(params.api);
    }

    return (
        <div className="container mt-4">
            <div className="row mb-4">
                <div className="col">
                    <div className="card">
                        <div className="card-header">
                            <a className="btn" href="/clients">Clients</a>
                        </div>
                        <div className="card-body data-grid">
                            <ClientDataGrid isLoading={isClientsLoading}
                                setIsLoading={setIsClientsLoading}
                                recordCount={clientsRecordCount}
                                setRecordCount={setClientsRecordCount}
                                onGridReady={onClientsGridReady} />
                        </div>
                    </div>
                </div>
                <div className="col">
                    <div className="card">
                        <div className="card-header">
                            <a className="btn" href="/users">Users</a>
                        </div>
                        <div className="card-body data-grid">
                            <UserDataGrid isLoading={isUsersLoading}
                                setIsLoading={setIsUsersLoading}
                                recordCount={usersRecordCount}
                                setRecordCount={setUsersRecordCount}
                                onGridReady={onUsersGridReady} />
                        </div>
                    </div>
                </div>
            </div >
            <div className="card">
                <div className="card-header">
                    <a className="btn" href="/workitems">Work Items</a>
                </div>
                <div className="card-body data-grid">
                    <WorkItemDataGrid isLoading={isWorkItemsLoading}
                        setIsLoading={setIsWorkItemsLoading}
                        recordCount={workItemsRecordCount}
                        setRecordCount={setWorkItemsRecordCount}
                        onGridReady={onWorkItemsGridReady} />
                </div>
            </div >
        </div >
    );
}

export default Home;