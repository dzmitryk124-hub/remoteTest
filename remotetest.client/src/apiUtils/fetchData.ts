import type { Forecast } from './../models/Forecast';

export const fetchData = async (setForecasts: React.Dispatch<React.SetStateAction<Forecast[] | undefined>>) => {
    const response = await fetch('weatherforecast');
    if (response.ok) {
        const data = await response.json();
        setForecasts(data);
    }
};