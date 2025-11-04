import React from "react";
import { Event } from "../types";

type Props = {
    event: Event;
    isAttending: boolean;
    onViewAttendees: (event: Event) => void;
    onAttend: (event: Event) => void;
    onLeave: (event: Event) => void;
};

const EventCard: React.FC<Props> = ({ event, isAttending, onViewAttendees, onAttend, onLeave }) => {
    return (
        <div
            style={{
                border: "1px solid #ddd",
                padding: 10,
                marginBottom: 10,
                borderRadius: 4,
                boxShadow: "0 2px 4px rgba(0,0,0,0.1)",
            }}
        >
            <h3 style={{ marginTop: 0 }}>{event.title}</h3>
            <p>{event.description}</p>
            <p>
                Date: {new Date(event.date).toLocaleDateString()} | Time: {event.startTime} - {event.endTime}
            </p>
            <p>Location: {event.location}</p>


            <div>
                <button
                    style={btnStyle}
                    onClick={() => onViewAttendees(event)}
                >
                    View Attendees
                </button>


                {isAttending ? (
                    <button
                        style={{ ...btnStyle, backgroundColor: "#dc3545" }}
                        onClick={() => onLeave(event)}
                    >
                        Leave Event
                    </button>
                ) : (
                    <button
                        style={{ ...btnStyle, backgroundColor: "#28a745" }}
                        onClick={() => onAttend(event)}
                    >
                        Attend Event
                    </button>
                )}
            </div>
        </div>
    );
};

const btnStyle: React.CSSProperties = {
    margin: 5,
    padding: "10px 15px",
    color: "#fff",
    border: "none",
    borderRadius: 4,
    cursor: "pointer",
    backgroundColor: "#0d6efd",
};


export default EventCard;