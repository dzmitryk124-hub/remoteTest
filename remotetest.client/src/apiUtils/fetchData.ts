import type { MeterReadingViewDto } from './../models/MeterReadingViewDto';

export const fetchData = async (setForecasts: React.Dispatch<React.SetStateAction<MeterReadingViewDto[] | undefined>>) => {
    const response = await fetch('meter-reading');
    if (response.ok) {
        const data = await response.json();
        setForecasts(data);
    }
};