import React from "react";
import { Attendee } from "../types";


type Props = { attendees: Attendee[] };


const AttendeesList: React.FC<Props> = ({ attendees }) => {
    if (!attendees.length) {
        return <p style={{ margin: 0 }}>No attendees yet.</p>;
    }
    return (
        <ul style={{ paddingLeft: 18, margin: 0 }}>
            {attendees.map((a) => (
                <li key={a.userId}>{a.userName}</li>
            ))}
        </ul>
    );
};


export default AttendeesList;