import { useEffect, useState } from 'react';
import './App.css';
import type { MeterReadingViewDto } from './models/MeterReadingViewDto';
import { fetchData } from './apiUtils/fetchData';

const App: React.FC = () => {
    const [dtos, setDtos] = useState<MeterReadingViewDto[]>();

    useEffect(() => {
        fetchData(setDtos);
    }, []);

    const contents = dtos === undefined
        ? (
            <p>
                <em>
                    Loading... Please refresh once the ASP.NET backend has started. See{' '}
                    <a href="https://aka.ms/jspsintegrationreact">
                        https://aka.ms/jspsintegrationreact
                    </a>{' '}
                    for more details.
                </em>
            </p>
        )
        : (
            <table className="table table-striped" aria-labelledby="tableLabel">
                <thead>
                    <tr>
                        <th>Id</th>
                        <th>First Name</th>
                        <th>Last Name</th>
                        <th>Date Time</th>
                        <th>Value</th>
                    </tr>
                </thead>
                <tbody>
                    {dtos.map(x => (
                        <tr key={x.id}>
                            <td>{x.id}</td>
                            <td>{x.firstName}</td>
                            <td>{x.lastName}</td>
                            <td>{x.meterReadingDateTime}</td>
                            <td>{x.meterReadValue}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        );

    return (
        <div>
            <h1 id="tableLabel">Meter Readings</h1>
            <p>This component demonstrates fetching data from the server.</p>
            {contents}
        </div>
    );
};

export default App;