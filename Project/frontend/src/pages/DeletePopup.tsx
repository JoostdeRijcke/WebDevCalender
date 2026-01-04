import React, { useState, useEffect } from "react";
import { useNavigate } from 'react-router-dom';
import '../styling/DeletePopup.css';

export const DeletePopup: React.FC<{ onDelete: () => void; onCancel: () => void }> = ({ onDelete, onCancel }) => {
  return (
    <div className="popup-overlay">
      <div className="popup-container">
        <div className="popup-message">Are you sure you want to delete this event?</div>
        <button className="popup-cancel" onClick={onCancel}>Cancel</button>
        <button className="popup-delete" onClick={onDelete}>Delete</button>
      </div>
    </div>
  );
};

type Event = {
  id: number;
  title: string;
  description: string;
  date: string;
  startTime: string;
  endTime: string;
  location: string;
};

export const DeleteEvent: React.FC = () => {
  const [showPopup, setShowPopup] = useState(false);
  const [eventId, setEventId] = useState(""); // State to store the event ID
  const [events, setEvents] = useState<Event[]>([]);
  const [selectedEventId, setSelectedEventId] = useState<number | null>(null);
  const [useManualEntry, setUseManualEntry] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    fetchRecentEvents();
  }, []);

  const fetchRecentEvents = async () => {
    try {
      const response = await fetch('http://localhost:5001/api/Events', {
        method: 'GET',
        credentials: 'include',
      });

      if (response.ok) {
        const data = await response.json();
        // Sort by ID descending to show most recent first
        const sortedEvents = data.sort((a: Event, b: Event) => b.id - a.id);
        setEvents(sortedEvents);
      } else {
        console.error('Failed to fetch events');
      }
    } catch (error) {
      console.error('Error fetching events:', error);
    }
  };

  // Function to handle the deletion action
  const handleDeleteEvent = async () => {
    const idToDelete = useManualEntry ? eventId : selectedEventId?.toString();

    if (!idToDelete) {
      alert("Please select or enter an event ID.");
      return;
    }

    try {
      const response = await fetch(`http://localhost:5001/api/Events/${idToDelete}`, {
        method: "DELETE",
        headers: {
          "Content-Type": "application/json",
        },
        credentials: 'include',
      });

      if (response.ok) {
        alert("Event has been deleted.");
        setShowPopup(false);
        setEventId("");
        setSelectedEventId(null);
        fetchRecentEvents(); // Refresh the list
      } else {
        const errorText = await response.text();
        console.error('Error deleting event:', errorText);
        alert("Failed to delete the event: " + errorText);
      }

    } catch (error) {
      console.error('Network error:', error);
      alert("Failed to delete the event.");
    }
  };

  // Function to handle cancel action
  const handleCancelDelete = () => {
    setShowPopup(false);
  };

  // Function to show the popup
  const handleShowDeletePopup = () => {
    if (useManualEntry) {
      if (!eventId.trim()) {
        alert("Please enter an event ID before attempting to delete.");
        return;
      }
    } else {
      if (selectedEventId === null) {
        alert("Please select an event from the list.");
        return;
      }
    }
    setShowPopup(true);
  };

  const handleEventSelect = (id: number) => {
    setSelectedEventId(id);
    setUseManualEntry(false);
  };

  return (
    <div className="container">
      <h1>Delete Event</h1>

      <div style={{ marginBottom: '20px' }}>
        <button
          onClick={() => setUseManualEntry(!useManualEntry)}
          style={{
            padding: '10px 20px',
            marginBottom: '15px',
            backgroundColor: '#4CAF50',
            color: 'white',
            border: 'none',
            borderRadius: '4px',
            cursor: 'pointer',
            transition: 'background-color 0.3s'
          }}
          onMouseEnter={(e) => e.currentTarget.style.backgroundColor = '#45a049'}
          onMouseLeave={(e) => e.currentTarget.style.backgroundColor = '#4CAF50'}
        >
          {useManualEntry ? 'Switch to Event List' : 'Enter Event ID Manually'}
        </button>
      </div>

      {useManualEntry ? (
        <div>
          <input
            className="input"
            type="text"
            placeholder="Enter Event ID"
            value={eventId}
            onChange={(e) => {
              setEventId(e.target.value);
              setSelectedEventId(null);
            }}
          />
        </div>
      ) : (
        <div style={{ maxHeight: '400px', overflowY: 'auto', marginBottom: '20px' }}>
          <h3>Select an event to delete:</h3>
          {events.length === 0 ? (
            <p>No events available.</p>
          ) : (
            <div>
              {events.map((event) => (
                <div
                  key={event.id}
                  onClick={() => handleEventSelect(event.id)}
                  style={{
                    padding: '15px',
                    margin: '10px 0',
                    border: selectedEventId === event.id ? '2px solid #007bff' : '1px solid #ddd',
                    borderRadius: '5px',
                    cursor: 'pointer',
                    backgroundColor: selectedEventId === event.id ? '#e7f3ff' : 'white',
                    transition: 'all 0.2s'
                  }}
                >
                  <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'start' }}>
                    <div style={{ flex: 1 }}>
                      <strong style={{ fontSize: '1.1em' }}>{event.title}</strong>
                      <p style={{ margin: '5px 0', color: '#666' }}>
                        {event.date.split('T')[0]} | {event.startTime.substring(0, 5)} - {event.endTime.substring(0, 5)}
                      </p>
                      <p style={{ margin: '5px 0', color: '#888', fontSize: '0.9em' }}>
                        {event.location}
                      </p>
                    </div>
                    <div style={{
                      backgroundColor: '#f0f0f0',
                      padding: '5px 10px',
                      borderRadius: '4px',
                      fontSize: '0.9em',
                      color: '#666'
                    }}>
                      ID: {event.id}
                    </div>
                  </div>
                </div>
              ))}
            </div>
          )}
        </div>
      )}

      <button
        className="trigger-button"
        onClick={handleShowDeletePopup}
        style={{
          padding: '10px 20px',
          backgroundColor: '#f44336',
          color: 'white',
          border: 'none',
          borderRadius: '4px',
          cursor: 'pointer',
          fontSize: '16px',
          transition: 'background-color 0.3s'
        }}
        onMouseEnter={(e) => e.currentTarget.style.backgroundColor = '#da190b'}
        onMouseLeave={(e) => e.currentTarget.style.backgroundColor = '#f44336'}
      >
        Delete Event
      </button>

      <button
        onClick={() => navigate('/calender')}
        style={{
          padding: '10px 20px',
          marginLeft: '10px',
          backgroundColor: '#2196F3',
          color: 'white',
          border: 'none',
          borderRadius: '4px',
          cursor: 'pointer',
          fontSize: '16px',
          transition: 'background-color 0.3s'
        }}
        onMouseEnter={(e) => e.currentTarget.style.backgroundColor = '#0b7dda'}
        onMouseLeave={(e) => e.currentTarget.style.backgroundColor = '#2196F3'}
      >
        Back to Calendar
      </button>

      {/* Popup will be shown if showPopup is true */}
      {showPopup && (
        <DeletePopup
          onDelete={handleDeleteEvent}
          onCancel={handleCancelDelete}
        />
      )}
    </div>
  );
};
