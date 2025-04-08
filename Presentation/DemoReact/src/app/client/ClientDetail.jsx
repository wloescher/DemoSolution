import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useState } from "react";
import { useParams, Link } from 'react-router-dom';

// Functions
import { useLoadData } from '../functions';

const ClientDetail = () => {
    const [data = [], setData] = useState();
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState();
    const params = useParams();
    const id = parseInt(params.id);

    // ------------------------------------------------------------
    // Load data
    // ------------------------------------------------------------

    useLoadData('/test/client/' + id, setIsLoading, setData, setError);

    // ------------------------------------------------------------
    // Presentation Layer
    // ------------------------------------------------------------

    return (
        <>
            <div className="container">
                <div className="row">
                    <div className="col">
                        <h1>View Client</h1>
                    </div>
                    <div className="col text-end mt-2">
                        <Link to="/clients" className="btn btn-light border me-2">
                            <FontAwesomeIcon icon="fa-solid fa-x" className="me-2" /> Cancel
                        </Link>
                        <Link to={`/client/${id}/edit`} className="btn btn-primary">
                            <FontAwesomeIcon icon="fa-solid fa-pencil" className="me-2" /> Edit
                        </Link>
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
                            Name
                        </div>
                        <div className="col-10 col-value">
                            <div>{data.name}</div>
                        </div>
                    </div>
                    <div className="row data-row">
                        <div className="col-2 col-key">
                            Address (Line 1)
                        </div>
                        <div className="col-10 col-value">
                            <div>{data.addressLine1}</div>
                        </div>
                    </div>
                    <div className="row data-row">
                        <div className="col-2 col-key">
                            Address (Line 2)
                        </div>
                        <div className="col-10 col-value">
                            <div>{data.addressLine2}</div>
                        </div>
                    </div>
                    <div className="row data-row">
                        <div className="col-2 col-key">
                            City
                        </div>
                        <div className="col-10 col-value">
                            <div>{data.city}</div>
                        </div>
                    </div>
                    <div className="row data-row">
                        <div className="col-2 col-key">
                            Region
                        </div>
                        <div className="col-10 col-value">
                            <div>{data.region}</div>
                        </div>
                    </div>
                    <div className="row data-row">
                        <div className="col-2 col-key">
                            Postal Code
                        </div>
                        <div className="col-10 col-value">
                            <div>{data.postalCode}</div>
                        </div>
                    </div>
                    <div className="row data-row">
                        <div className="col-2 col-key">
                            Country
                        </div>
                        <div className="col-10 col-value">
                            <div>{data.country}</div>
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
                    <div className="row data-row">
                        <div className="col-2 col-key">
                            URL / Website
                        </div>
                        <div className="col-10 col-value">
                            <a href="{{ data.url }}" target="_blank">{data.url}</a>
                        </div>
                    </div>
                </div>
            </div >
        </>
    );
}

export default ClientDetail;