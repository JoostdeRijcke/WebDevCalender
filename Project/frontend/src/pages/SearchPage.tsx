import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom'; // Ensure this is correctly imported

interface Event {
  id: number;
  title: string;
  description: string;
  date: string;
  startTime: string;
  endTime: string;
  location: string;
}

interface Attendee {
  userId: number;
  userName: string;
}

const SearchPage: React.FC = () => {
  const [events, setEvents] = useState<Event[]>([]);
  const [filteredEvents, setFilteredEvents] = useState<Event[]>([]);
  const [searchQuery, setSearchQuery] = useState<string>('');
  const [attendees, setAttendees] = useState<Attendee[]>([]);
  const [popupEvent, setPopupEvent] = useState<Event | null>(null);
  const [noAttendeesPopup, setNoAttendeesPopup] = useState<string | null>(null);
  const [confirmationMessage, setConfirmationMessage] = useState<string | null>(null);
  const [loggedInUserId] = useState<number>(2); // Mock logged-in user ID for testing
  const [attendedEvents, setAttendedEvents] = useState<number[]>([]);

  const navigate = useNavigate(); // Ensure this hook is used

  // // Check if user is logged in
  // useEffect(() => {
  //   const checkUserLoggedIn = async () => {
  //     try {
  //       const response = await fetch('http://localhost:5001/api/IsUserLoggedIn', {
  //         credentials: 'include',
  //       });
  //       if (response.status !== 200) {
  //         navigate('/'); // Redirect to login page if not logged in
  //       }
  //     } catch (error) {
  //       console.error('Error checking user login status:', error);
  //       navigate('/'); // Redirect to login on error
  //     }
  //   };

  //   checkUserLoggedIn();
  // }, [navigate]);
  useEffect(() => {
    const fetchEvents = async () => {
      try {
        const response = await fetch('http://localhost:5001/api/Events');
        if (!response.ok) {
          throw new Error(`Error: ${response.status}`);
        }
        const data: Event[] = await response.json();
        setEvents(data);
        setFilteredEvents(data);
      } catch (error) {
        console.error('Error fetching events:', error);
      }
    };

    const fetchAttendedEvents = async () => {
      try {
        const response = await fetch(
          `http://localhost:5001/api/EventAttendance/user/${loggedInUserId}/attended-events`
        );
        if (!response.ok) {
          throw new Error(`Error: ${response.status}`);
        }
        const data: Event[] = await response.json();
        const attendedEventIds = data.map((event) => event.id);
        setAttendedEvents(attendedEventIds);
      } catch (error) {
        console.error('Error fetching attended events:', error);
      }
    };

    fetchEvents();
    fetchAttendedEvents();
  }, [loggedInUserId]);

  useEffect(() => {
    const filtered = events.filter((event) =>
      event.title.toLowerCase().includes(searchQuery.toLowerCase())
    );
    setFilteredEvents(filtered);
  }, [searchQuery, events]);

  const handleSearch = (e: React.ChangeEvent<HTMLInputElement>) => {
    setSearchQuery(e.target.value);
  };

  const handleViewAttendees = async (event: Event) => {
    try {
      const response = await fetch(
        `http://localhost:5001/api/EventAttendance/attendees/${event.id}`
      );
      if (!response.ok) {
        throw new Error(`Error: ${response.status}`);
      }
      const data: Attendee[] = await response.json();
      if (data.length === 0) {
        setNoAttendeesPopup(`No attendees for "${event.title}" yet.`);
      } else {
        setAttendees(data);
        setPopupEvent(event);
      }
    } catch (error) {
      console.error('Error fetching attendees:', error);
    }
  };

  const handleAttendEvent = async (event: Event) => {
    try {
      const response = await fetch('http://localhost:5001/api/EventAttendance/attend', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ userId: loggedInUserId, eventId: event.id }),
      });
      if (!response.ok) {
        throw new Error(`Error: ${response.status}`);
      }
      setAttendedEvents([...attendedEvents, event.id]);
      setConfirmationMessage(`You have successfully attended "${event.title}".`);
    } catch (error) {
      console.error('Error attending event:', error);
    }
  };

  const handleLeaveEvent = async (event: Event) => {
    try {
      const response = await fetch('http://localhost:5001/api/EventAttendance/remove', {
        method: 'DELETE',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ userId: loggedInUserId, eventId: event.id }),
      });
      if (!response.ok) {
        throw new Error(`Error: ${response.status}`);
      }
      setAttendedEvents(attendedEvents.filter((id) => id !== event.id));
      setConfirmationMessage(`You have successfully left "${event.title}".`);
    } catch (error) {
      console.error('Error leaving event:', error);
    }
  };

  return (
    <div style={styles.container}>
      <div style={styles.popup}>
        <h1 style={styles.header}>Search Events</h1>
        <input
          type="text"
          placeholder="Search for events"
          value={searchQuery}
          onChange={handleSearch}
          style={styles.searchInput}
        />
        <div style={styles.eventList}>
          {filteredEvents.length > 0 ? (
            filteredEvents.map((event) => (
              <div key={event.id} style={styles.eventItem}>
                <h3>{event.title}</h3>
                <p>{event.description}</p>
                <p>
                  Date: {new Date(event.date).toLocaleDateString()} | Time:{' '}
                  {event.startTime} - {event.endTime}
                </p>
                <p>Location: {event.location}</p>
                <button
                  style={styles.button}
                  onClick={() => handleViewAttendees(event)}
                >
                  View Attendees
                </button>
                {attendedEvents.includes(event.id) ? (
                  <button
                    style={{ ...styles.button, backgroundColor: 'red' }}
                    onClick={() => handleLeaveEvent(event)}
                  >
                    Leave Event
                  </button>
                ) : (
                  <button
                    style={{ ...styles.button, backgroundColor: 'green' }}
                    onClick={() => handleAttendEvent(event)}
                  >
                    Attend Event
                  </button>
                )}
              </div>
            ))
          ) : (
            <p>No events found.</p>
          )}
        </div>
      </div>

      {/* Attendees Popup */}
      {popupEvent && (
        <div style={styles.attendeesPopup}>
          <h2>Attendees for "{popupEvent.title}"</h2>
          <ul>
            {attendees.map((attendee) => (
              <li key={attendee.userId}>{attendee.userName}</li>
            ))}
          </ul>
          <button
            style={{ ...styles.button, backgroundColor: 'gray' }}
            onClick={() => setPopupEvent(null)}
          >
            Close
          </button>
        </div>
      )}

      {/* No Attendees Popup */}
      {noAttendeesPopup && (
        <div style={styles.noAttendeesPopup}>
          <p>{noAttendeesPopup}</p>
          <button
            style={{ ...styles.button, backgroundColor: 'gray' }}
            onClick={() => setNoAttendeesPopup(null)}
          >
            Close
          </button>
        </div>
      )}

      {/* Confirmation Popup */}
      {confirmationMessage && (
        <div style={styles.confirmationPopup}>
          <p>{confirmationMessage}</p>
          <button
            style={{ ...styles.button, backgroundColor: 'gray' }}
            onClick={() => setConfirmationMessage(null)}
          >
            Close
          </button>
        </div>
      )}
    </div>
  );
};

const styles: { [key: string]: React.CSSProperties } = {
  container: {
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center',
    height: '100vh',
    backgroundColor: '#f8f9fa',
  },
  popup: {
    width: '100%',
    maxWidth: '800px',
    backgroundColor: '#fff',
    border: '1px solid #ccc',
    borderRadius: '8px',
    boxShadow: '0 4px 8px rgba(0, 0, 0, 0.1)',
    overflow: 'hidden',
    display: 'flex',
    flexDirection: 'column',
    maxHeight: '90vh',
  },
  header: {
    margin: '0',
    padding: '16px',
    fontSize: '24px',
    borderBottom: '1px solid #ddd',
    textAlign: 'center',
    backgroundColor: '#f5f5f5',
  },
  searchInput: {
    width: 'calc(100% - 32px)',
    margin: '16px',
    padding: '10px',
    fontSize: '16px',
    border: '1px solid #ccc',
    borderRadius: '4px',
  },
  eventList: {
    overflowY: 'auto',
    padding: '16px',
    flex: '1',
  },
  eventItem: {
    border: '1px solid #ddd',
    padding: '10px',
    marginBottom: '10px',
    borderRadius: '4px',
    boxShadow: '0 2px 4px rgba(0, 0, 0, 0.1)',
  },
  button: {
    margin: '5px',
    padding: '10px 15px',
    color: '#fff',
    border: 'none',
    borderRadius: '4px',
    cursor: 'pointer',
  },
  attendeesPopup: {
    position: 'fixed',
    top: '20%',
    left: '50%',
    transform: 'translate(-50%, -20%)',
    width: '400px',
    backgroundColor: '#fff',
    border: '1px solid #ccc',
    borderRadius: '8px',
    padding: '16px',
    boxShadow: '0 4px 8px rgba(0, 0, 0, 0.2)',
  },
  confirmationPopup: {
    position: 'fixed',
    top: '30%',
    left: '50%',
    transform: 'translate(-50%, -30%)',
    width: '300px',
    backgroundColor: '#fff',
    border: '1px solid #ccc',
    borderRadius: '8px',
    padding: '16px',
    textAlign: 'center',
    boxShadow: '0 4px 8px rgba(0, 0, 0, 0.2)',
  },
  noAttendeesPopup: {
    position: 'fixed',
    top: '30%',
    left: '50%',
    transform: 'translate(-50%, -30%)',
    width: '300px',
    backgroundColor: '#fff',
    border: '1px solid #ccc',
    borderRadius: '8px',
    padding: '16px',
    textAlign: 'center',
    boxShadow: '0 4px 8px rgba(0, 0, 0, 0.2)',
  },
};

export default SearchPage;
