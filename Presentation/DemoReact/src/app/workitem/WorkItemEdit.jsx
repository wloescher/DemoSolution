import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useState } from "react";
import { useParams } from 'react-router-dom';

// Functions
import { useLoadData } from '../functions';

const WorkItemEdit = () => {
    const [data = [], setData] = useState();
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState();
    const params = useParams();
    const id = params.id ? parseInt(params.id) : 0;

    const save = () => {
        alert("TODO: Save changes.");
    }

    // ------------------------------------------------------------
    // Load data
    // ------------------------------------------------------------

    // TODO: Swap out WebAPI URL to 'workItem/' + id
    useLoadData('/test/workItem/' + id, setIsLoading, setData, setError);

    // ------------------------------------------------------------
    // Presentation Layer
    // ------------------------------------------------------------

    return (
        <>
            <div className="container">
                <div className="row">
                    <div className="col">
                        <h1>{ id == 0 ? "Add" : "Edit" } Work Item</h1>
                    </div>
                    <div className="col text-end mt-2">
                        <button className="btn btn-primary" onClick={() => save()}>
                            <FontAwesomeIcon icon="fa-solid fa-save" className="me-2" /> Save
                        </button>
                    </div>
                </div>
                <div className="container data">
                    <div className="row data-row">
                        <div className="col-2 col-key">
                            Id
                        </div>
                        <div className="col-10 col-value">
                            {id}
                        </div>
                    </div>
                    <div className="row data-row">
                        <div className="col-2 col-key">
                            Guid
                        </div>
                        <div className="col-10 col-value">
                            {data.guid}
                        </div>
                    </div>
                    <div className="row data-row">
                        <div className="col-2 col-key">
                            TypeId
                        </div>
                        <div className="col-10 col-value">
                            {data.typeId}
                        </div>
                    </div>
                    <div className="row data-row">
                        <div className="col-2 col-key">
                            Type
                        </div>
                        <div className="col-10 col-value">
                            {data.type}
                        </div>
                    </div>
                    <div className="row data-row">
                        <div className="col-2 col-key">
                            StatusId
                        </div>
                        <div className="col-10 col-value">
                            {data.statusId}
                        </div>
                    </div>
                    <div className="row data-row">
                        <div className="col-2 col-key">
                            Status
                        </div>
                        <div className="col-10 col-value">
                            {data.status}
                        </div>
                    </div>
                    <div className="row data-row">
                        <div className="col-2 col-key">
                            Title
                        </div>
                        <div className="col-10 col-value">
                            {data.title}
                        </div>
                    </div>
                    <div className="row data-row">
                        <div className="col-2 col-key">
                            Sub-Title
                        </div>
                        <div className="col-10 col-value">
                            {data.subTitle}
                        </div>
                    </div>
                    <div className="row data-row">
                        <div className="col-2 col-key">
                            Summary
                        </div>
                        <div className="col-10 col-value">
                            {data.summary}
                        </div>
                    </div>
                    <div className="row data-row">
                        <div className="col-2 col-key">
                            Body
                        </div>
                        <div className="col-10 col-value">
                            {data.body}
                        </div>
                    </div>
                </div>
            </div >
        </>
    );
}

export default WorkItemEdit;