import { Event } from "../types";


const BASE_URL = "http://localhost:5001/api";


export async function getAttendedEventsByUser(userId: number): Promise<Event[]> {
    const res = await fetch(`${BASE_URL}/EventAttendance/user/${userId}/attended-events`);
    if (!res.ok) throw new Error(`getAttendedEventsByUser failed: ${res.status}`);
    return res.json();
}


export async function attendEvent(userId: number, eventId: number): Promise<void> {
    const res = await fetch(`${BASE_URL}/EventAttendance/attend`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ userId, eventId }),
    });
    if (!res.ok) throw new Error(`attendEvent failed: ${res.status}`);
}


export async function leaveEvent(userId: number, eventId: number): Promise<void> {
    const res = await fetch(`${BASE_URL}/EventAttendance/remove`, {
        method: "DELETE",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ userId, eventId }),
    });
    if (!res.ok) throw new Error(`leaveEvent failed: ${res.status}`);
}