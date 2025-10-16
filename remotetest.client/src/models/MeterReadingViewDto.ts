export interface MeterReadingViewDto {
  id: number;
  accountId: number;
  meterReadingDateTime: string; // Use string for ISO date/time, or Date if you will parse it
  meterReadValue: string;       // Format can be handled in UI if needed
  firstName: string;
  lastName: string;
}