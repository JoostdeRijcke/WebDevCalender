import React from "react";
import { Event } from "../types";
import EventCard from "./EventCard";

type Props = {
    events: Event[];
    attendedEventIds: number[];
    onViewAttendees: (event: Event) => void;
    onAttend: (event: Event) => void;
    onLeave: (event: Event) => void;
};

const EventsList: React.FC<Props> = ({ events, attendedEventIds, onViewAttendees, onAttend, onLeave }) => {
    if (!events.length) return <p>No events found.</p>;


    return (
        <div style={{ overflowY: "auto", padding: 16, flex: 1 }}>
            {events.map((ev) => (
                <EventCard
                    key={ev.id}
                    event={ev}
                    isAttending={attendedEventIds.includes(ev.id)}
                    onViewAttendees={onViewAttendees}
                    onAttend={onAttend}
                    onLeave={onLeave}
                />
            ))}
        </div>
    );
};


export default EventsList;