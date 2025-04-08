import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useState } from "react";
import WorkItemDataGrid from "../workItem/WorkItemDataGrid"
import { useLocation, Link } from 'react-router-dom';

function WorkItemList() {
    const [isWorkItemsLoading, setIsWorkItemsLoading] = useState();
    const [workItemsRecordCount, setWorkItemsRecordCount] = useState();
    const [workItemsGridRef, setWorkItemsGridRef] = useState();
    const location = useLocation();

    const getButtonClassName = (path) => {
        var className = 'btn btn-outline-secondary';
        if (location.pathname === path) {
            className += ' bg-info';
        }
        return className;
    };

    function onWorkItemsGridReady(params) {
        setWorkItemsGridRef(params.api);
    }

    return (
        <>
            <div className="container">
                <div className="row">
                    <div className="col">
                        <h1>Work Item List</h1>
                    </div>
                    <div className="col text-end">
                        <div className="btn-group border" role="group" >
                            <Link to="/workitems" className={getButtonClassName('/workitems')}>ALL</Link>
                            <Link to="/workitems/user-story" className={getButtonClassName('/workitems/user-story')}>User Stories</Link>
                            <Link to="/workitems/task" className={getButtonClassName('/workitems/task')}>Tasks</Link>
                            <Link to="/workitems/bug" className={getButtonClassName('/workitems/bug')}>Bugs</Link>
                            <Link to="/workitems/epic" className={getButtonClassName('/workitems/epic')}>Epic</Link>
                            <Link to="/workitems/feature" className={getButtonClassName('/workitems/feature')}>Features</Link>
                        </div>
                    </div>
                    <div className="col-1 text-end">
                        <Link to="/workitem/add" className="btn btn-primary">
                            <FontAwesomeIcon icon="fa-solid fa-plus" className="me-2" /> Add
                        </Link>
                    </div>
                </div>
                <div className="data-grid">
                    <WorkItemDataGrid isLoading={isWorkItemsLoading}
                        setIsLoading={setIsWorkItemsLoading}
                        recordCount={workItemsRecordCount}
                        setRecordCount={setWorkItemsRecordCount}
                        onGridReady={onWorkItemsGridReady} />
                </div>
            </div>
        </>

    );
}

export default WorkItemList;