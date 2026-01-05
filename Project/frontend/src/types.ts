export interface Event {
    id: number;
    title: string;
    description: string;
    date: string; // ISO date string
    startTime: string; // e.g. "09:00"
    endTime: string; // e.g. "10:00"
    location: string;
    maxAttendees?: number | null;
    eventAttendances?: Array<{ userId: number; userName: string }>;
}


export interface Attendee {
    userId: number;
    userName: string;
}