import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useState } from "react";
import { useParams, Link } from "react-router-dom";

// Functions
import { useLoadData } from "../functions";
import { getWorkItemTypes } from "../functions";
import { getWorkItemStatuses } from "../functions";

const WorkItemEdit = () => {
    const [data = [], setData] = useState();
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState();
    const params = useParams();
    const id = params.id ? parseInt(params.id) : 0;

    const saveChanges = () => {
        alert("TODO: Save changes.");
    };

    const deleteItem = () => {
        alert("TODO: Delete item.");
    };

    // ------------------------------------------------------------
    // Load data
    // ------------------------------------------------------------

    // TODO: Swap out WebAPI URL to 'workItem/' + id
    useLoadData("/test/workItem/" + id, setIsLoading, setData, setError);

    // ------------------------------------------------------------
    // Presentation Layer
    // ------------------------------------------------------------

    return (
        <>
            <div className="container">
                <div className="row">
                    <div className="col">
                        <h1>{id == 0 ? "Add" : "Edit"} Work Item</h1>
                    </div>
                    <div className="col text-end mt-2">
                        <Link to={`/workitem/${id}`} className="btn btn-light border me-2">
                            <FontAwesomeIcon icon="fa-solid fa-x" className="me-2" /> Cancel
                        </Link>
                        <button type="submit" className="btn btn-primary" onClick={() => saveChanges()}>
                            <FontAwesomeIcon icon="fa-solid fa-save" className="me-2" /> Save
                        </button>
                    </div>
                </div>
                <div className="row data-row">
                    <div className="col-6">
                        <div className="mb-3">
                            <label htmlFor="typeId" className="form-label">
                                Title
                            </label>
                            <input id="title" type="text" className="form-control required" aria-label="Title" value={data.title ?? ""} required
                                onChange={(e) =>
                                    setData({
                                        ...data,
                                        title: e.target.value,
                                    })
                                }
                            />
                        </div>
                        <div className="mb-3">
                            <label htmlFor="typeId" className="form-label">
                                Work Item Type
                            </label>
                            <select id="typeId" className="form-select required" aria-label="Type" value={data.typeId ?? 0} required
                                onChange={(e) =>
                                    setData({
                                        ...data,
                                        typeId: e.target.value,
                                    })
                                }
                            >
                                {getWorkItemTypes().map((item) => (
                                    <option key={item.value} value={item.value}>
                                        {item.label}
                                    </option>
                                ))}
                            </select>
                        </div>
                        <div className="mb-3">
                            <label htmlFor="statusId" className="form-label">
                                Work Item Status
                            </label>
                            <select id="statusId" className="form-select required" aria-label="Status" value={data.statusId ?? 0} required
                                onChange={(e) =>
                                    setData({
                                        ...data,
                                        statusId: e.target.value,
                                    })
                                }
                            >
                                {getWorkItemStatuses().map((item) => (
                                    <option key={item.value} value={item.value}>
                                        {item.label}
                                    </option>
                                ))}
                            </select>
                        </div>
                    </div>
                    <div className="col-6">
                        <div className="mb-3">
                            <input id="subTitle" type="text" className="form-control" aria-label="Sub-Title" value={data.subTitle ?? ""}
                                onChange={(e) =>
                                    setData({
                                        ...data,
                                        subTitle: e.target.value,
                                    })
                                }
                            />
                        </div>
                        <div className="mb-3">
                            <textarea id="summary" type="text" className="form-control" aria-label="Summary" value={data.summary ?? ""} rows="3"
                                onChange={(e) =>
                                    setData({
                                        ...data,
                                        summary: e.target.value,
                                    })
                                }
                            />
                        </div>
                        <div className="mb-3">
                            <textarea id="body" type="text" className="form-control" aria-label="Body" value={data.body ?? ""} rows="10"
                                onChange={(e) =>
                                    setData({
                                        ...data,
                                        body: e.target.value,
                                    })
                                }
                            />
                        </div>
                    </div>
                </div>

                <div className={id == 0 ? "hidden" : "row"}>
                    <div className="col text-end">
                        <button
                            type="submit"
                            className="btn btn-danger"
                            onClick={() => deleteItem()}
                        >
                            <FontAwesomeIcon icon="fa-solid fa-trash" className="me-2" />{" "}
                            Delete
                        </button>
                    </div>
                </div>
            </div >
        </>
    );
};

export default WorkItemEdit;
