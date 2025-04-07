import { useState } from "react";
import ClientList from "../client/ClientList"
import UserList from "../user/UserList"
import WorkItemList from "../workitem/WorkItemList"

function Home() {
    // Clients
    const [isClientsLoading, setIsClientsLoading] = useState();
    const [ClientsRecordCount, setClientsRecordCount] = useState();
    const [ClientsGridRef, setClientsGridRef] = useState();

    // Users
    const [isUsersLoading, setIsUsersLoading] = useState();
    const [UsersRecordCount, setUsersRecordCount] = useState();
    const [UsersGridRef, setUsersGridRef] = useState();

    // WorkItems
    const [isWorkItemsLoading, setIsWorkItemsLoading] = useState();
    const [WorkItemsRecordCount, setWorkItemsRecordCount] = useState();
    const [WorkItemsGridRef, setWorkItemsGridRef] = useState();

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
                            <ClientList isLoading={isClientsLoading}
                                setIsLoading={setIsClientsLoading}
                                recordCount={ClientsRecordCount}
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
                            <UserList isLoading={isUsersLoading}
                                setIsLoading={setIsUsersLoading}
                                recordCount={UsersRecordCount}
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
                    <WorkItemList isLoading={isWorkItemsLoading}
                        setIsLoading={setIsWorkItemsLoading}
                        recordCount={WorkItemsRecordCount}
                        setRecordCount={setWorkItemsRecordCount}
                        onGridReady={onWorkItemsGridReady} />
                </div>
            </div >
        </div >
    );
}

export default Home;