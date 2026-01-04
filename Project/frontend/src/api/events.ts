import { Event, Attendee } from "../types";


const BASE_URL = "http://localhost:5001/api";


export async function getEvents(onlyUpcoming = true): Promise<Event[]> {
    const url = `${BASE_URL}/Events?onlyUpcoming=${onlyUpcoming ? "true" : "false"}`;
    const res = await fetch(url);
    if (!res.ok) throw new Error(`getEvents failed: ${res.status}`);
    return res.json();
}



export async function getEventAttendees(eventId: number): Promise<Attendee[]> {
    const res = await fetch(`${BASE_URL}/EventAttendance/attendees/${eventId}`);
    if (!res.ok) throw new Error(`getEventAttendees failed: ${res.status}`);
    return res.json();
}