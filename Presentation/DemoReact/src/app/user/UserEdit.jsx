import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useState } from "react";
import { useParams } from 'react-router-dom';

// Functions
import { useLoadData } from '../functions';

const UserEdit = () => {
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

    useLoadData('/test/user/' + id, setIsLoading, setData, setError);

    // ------------------------------------------------------------
    // Presentation Layer
    // ------------------------------------------------------------

    return (
        <>
            <div className="container">
                <div className="row">
                    <div className="col">
                        <h1>{id == 0 ? "Add" : "Edit"} User</h1>
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
                            Email Address
                        </div>
                        <div className="col-10 col-value">
                            <a href="mailto:{{ data.emailAddress }}">{data.emailAddress}</a>
                        </div>
                    </div>
                    <div className="row data-row">
                        <div className="col-2 col-key">
                            First Name
                        </div>
                        <div className="col-10 col-value">
                            {data.firstName}
                        </div>
                    </div>
                    <div className="row data-row">
                        <div className="col-2 col-key">
                            Middle Name
                        </div>
                        <div className="col-10 col-value">
                            {data.middleName}
                        </div>
                    </div>
                    <div className="row data-row">
                        <div className="col-2 col-key">
                            Last Name
                        </div>
                        <div className="col-10 col-value">
                            {data.lastName}
                        </div>
                    </div>
                    <div className="row data-row">
                        <div className="col-2 col-key">
                            Address (Line 1)
                        </div>
                        <div className="col-10 col-value">
                            {data.addressLine1}
                        </div>
                    </div>
                    <div className="row data-row">
                        <div className="col-2 col-key">
                            Address (Line 2)
                        </div>
                        <div className="col-10 col-value">
                            {data.addressLine2}
                        </div>
                    </div>
                    <div className="row data-row">
                        <div className="col-2 col-key">
                            City
                        </div>
                        <div className="col-10 col-value">
                            {data.city}
                        </div>
                    </div>
                    <div className="row data-row">
                        <div className="col-2 col-key">
                            Region
                        </div>
                        <div className="col-10 col-value">
                            {data.region}
                        </div>
                    </div>
                    <div className="row data-row">
                        <div className="col-2 col-key">
                            Postal Code
                        </div>
                        <div className="col-10 col-value">
                            {data.postalCode}
                        </div>
                    </div>
                    <div className="row data-row">
                        <div className="col-2 col-key">
                            Country
                        </div>
                        <div className="col-10 col-value">
                            {data.country}
                        </div>
                    </div>
                    <div className="row data-row">
                        <div className="col-2 col-key">
                            Phone Number
                        </div>
                        <div className="col-10 col-value">
                            <a href="tel:{{ data.phoneNumber }}" target="_blank">{data.phoneNumber}</a>
                        </div>
                    </div>
                </div>
            </div >

        </>
    );
}

export default UserEdit;