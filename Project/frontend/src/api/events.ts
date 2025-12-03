import { Event, Attendee } from "../types";


const BASE_URL = "http://localhost:5001/api";


export async function getEvents(onlyUpcoming = true): Promise<Event[]> {
    const url = `${BASE_URL}/Events?onlyUpcoming=${onlyUpcoming ? "true" : "false"}`;
    console.log("=== DEBUG getEvents ===");
    console.log("onlyUpcoming parameter:", onlyUpcoming);
    console.log("Fetching URL:", url);

    const res = await fetch(url);
    console.log("Response status:", res.status);

    if (!res.ok) throw new Error(`getEvents failed: ${res.status}`);

    const data = await res.json();
    console.log("Events received:", data.length);
    console.log("=== END DEBUG ===");

    return data;
}



export async function getEventAttendees(eventId: number): Promise<Attendee[]> {
    const res = await fetch(`${BASE_URL}/EventAttendance/attendees/${eventId}`);
    if (!res.ok) throw new Error(`getEventAttendees failed: ${res.status}`);
    return res.json();
}