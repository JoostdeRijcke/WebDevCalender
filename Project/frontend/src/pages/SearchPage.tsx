// src/pages/SearchPage.tsx
import React, { useEffect, useMemo, useState } from "react";
import { useNavigate } from "react-router-dom";
import { Event, Attendee } from "../types";
import SearchBar from "../components/SearchBar";
import EventsList from "../components/EventsList";
import Modal from "../components/Modal";
import AttendeesList from "../components/AttendeesList";
import { getEvents, getEventAttendees } from "../api/events";
import { attendEvent, getAttendedEventsByUser, leaveEvent } from "../api/attendance";
import { isUpcoming, eventEndDate } from "../utils/EventTime"; // let op: hoofdletters zoals jouw bestand

const SearchPage: React.FC = () => {
    // ---------- State ----------
    const [events, setEvents] = useState<Event[]>([]);
    const [search, setSearch] = useState("");
    const [attendedIds, setAttendedIds] = useState<number[]>([]);

    const [attendees, setAttendees] = useState<Attendee[]>([]);
    const [activeEvent, setActiveEvent] = useState<Event | null>(null);

    const [infoMessage, setInfoMessage] = useState<string | null>(null);

    const [loggedInUserId] = useState<number>(2); // mock user
    const navigate = useNavigate();

    // ---------- Data laden ----------
    useEffect(() => {
        const load = async () => {
            try {
                const [evs, myEvs] = await Promise.all([
                    getEvents(),
                    getAttendedEventsByUser(loggedInUserId),
                ]);
                setEvents(evs);
                setAttendedIds(myEvs.map((e) => e.id));
            } catch (e) {
                console.error(e);
            }
        };
        load();
    }, [loggedInUserId]);

    // ---------- Sort helper (binnen component) ----------
    const byEndAsc = (a: Event, b: Event) => {
        const ta = eventEndDate(a)?.getTime() ?? Number.POSITIVE_INFINITY;
        const tb = eventEndDate(b)?.getTime() ?? Number.POSITIVE_INFINITY;
        return ta - tb;
    };

    // ---------- Filter + zoek ----------
    const filteredEvents = useMemo<Event[]>(() => {
        const now = new Date();

        const upcoming = events
            .filter((e: Event) => isUpcoming(e, now)) // alleen toekomstige events
            .slice()                                  // kopie, geen mutatie van state
            .sort(byEndAsc);                          // sorteer op eerstvolgende

        const q = (search ?? "").trim().toLowerCase();
        return q ? upcoming.filter((e: Event) => e.title.toLowerCase().includes(q)) : upcoming;
    }, [events, search]);

    // ---------- Handlers ----------
    const handleViewAttendees = async (event: Event) => {
        try {
            const list = await getEventAttendees(event.id);
            setAttendees(list);
            setActiveEvent(event);
            if (!list.length) {
                setInfoMessage(`No attendees for "${event.title}" yet.`);
            }
        } catch (e) {
            console.error(e);
        }
    };

    const handleAttend = async (event: Event) => {
        try {
            await attendEvent(loggedInUserId, event.id);
            setAttendedIds((prev) => (prev.includes(event.id) ? prev : [...prev, event.id]));
            setInfoMessage(`You have successfully attended "${event.title}".`);
        } catch (e) {
            console.error(e);
        }
    };

    const handleLeave = async (event: Event) => {
        try {
            await leaveEvent(loggedInUserId, event.id);
            setAttendedIds((prev) => prev.filter((id) => id !== event.id));
            setInfoMessage(`You have successfully left "${event.title}".`);
        } catch (e) {
            console.error(e);
        }
    };

    // ---------- Render ----------
    return (
        <div style={container}>
            <div style={panel}>
                <h1 style={header}>Search Events</h1>
                <SearchBar value={search} onChange={setSearch} />
                <EventsList
                    events={filteredEvents}
                    attendedEventIds={attendedIds}
                    onViewAttendees={handleViewAttendees}
                    onAttend={handleAttend}
                    onLeave={handleLeave}
                />
            </div>

            {/* Attendees modal */}
            <Modal
                isOpen={!!activeEvent}
                title={activeEvent ? `Attendees for "${activeEvent.title}"` : undefined}
                onClose={() => setActiveEvent(null)}
                width={420}
            >
                <AttendeesList attendees={attendees} />
            </Modal>

            {/* Info/confirmation modal */}
            <Modal isOpen={!!infoMessage} onClose={() => setInfoMessage(null)} width={360}>
                <p style={{ margin: 0 }}>{infoMessage}</p>
            </Modal>
        </div>
    );
};

// ---------- Styles ----------
const container: React.CSSProperties = {
    display: "flex",
    justifyContent: "center",
    alignItems: "center",
    height: "100vh",
    backgroundColor: "#f8f9fa",
};

const panel: React.CSSProperties = {
    width: "100%",
    maxWidth: 800,
    backgroundColor: "#fff",
    border: "1px solid #ccc",
    borderRadius: 8,
    boxShadow: "0 4px 8px rgba(0,0,0,0.1)",
    overflow: "hidden",
    display: "flex",
    flexDirection: "column",
    maxHeight: "90vh",
};

const header: React.CSSProperties = {
    margin: 0,
    padding: 16,
    fontSize: 24,
    borderBottom: "1px solid #ddd",
    textAlign: "center",
    backgroundColor: "#f5f5f5",
};

export default SearchPage;
