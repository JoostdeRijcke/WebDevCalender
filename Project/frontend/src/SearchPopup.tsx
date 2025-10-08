import React, { useState, useEffect, useRef } from 'react';

interface Event {
  id: number;
  title: string;
  description: string;
  date: string;
  startTime: string;
  endTime: string;
  location: string;
  attendees: number;
}

interface SearchPopupProps {
  isOpen: boolean;
  onClose: () => void;
  loggedInUserId: number;
}

interface Attendee {
  userId: number;
  userName: string;
  attendedAt: string;
}

type FilterType = 'dateRange' | 'location' | 'keyword';

interface Filter {
  type: FilterType;
  value: any;
}

const SearchPopup: React.FC<SearchPopupProps> = ({ isOpen, onClose, loggedInUserId }) => {
  const [events, setEvents] = useState<Event[]>([]);
  const [filteredEvents, setFilteredEvents] = useState<Event[]>([]);
  const [filters, setFilters] = useState<Filter[]>([]);
  const [newFilterType, setNewFilterType] = useState<FilterType>('dateRange');
  const [newFilterValue, setNewFilterValue] = useState<string>('');
  const [searchQuery, setSearchQuery] = useState<string>('');
  const [attendees, setAttendees] = useState<Attendee[]>([]);
  const [popupEvent, setPopupEvent] = useState<Event | null>(null);
  const [confirmationMessage, setConfirmationMessage] = useState<string | null>(null);
  const [attendedEvents, setAttendedEvents] = useState<number[]>([]);
  const [accordionOpen, setAccordionOpen] = useState<boolean>(false);
  const popupRef = useRef<HTMLDivElement>(null);
  const [viewAttendeesPopupOpen, setViewAttendeesPopupOpen] = useState<boolean>(false);

  useEffect(() => {
    const fetchEvents = async () => {
      try {
        const response = await fetch('http://localhost:5001/api/Events');
        if (!response.ok) throw new Error(`Error: ${response.status}`);
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
        if (!response.ok) throw new Error(`Error: ${response.status}`);
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
    applyFilters();
  }, [filters, searchQuery]);

  const applyFilters = () => {
    let filtered = [...events];

    filters.forEach((filter) => {
      if (filter.type === 'dateRange') {
        const [startDate, endDate] = filter.value.split(' to ').map((d: string) => new Date(d));
        filtered = filtered.filter((event) => {
          const eventDate = new Date(event.date);
          return eventDate >= startDate && eventDate <= endDate;
        });
      }
      if (filter.type === 'location') {
        filtered = filtered.filter((event) =>
          event.location.toLowerCase().includes(filter.value.toLowerCase())
        );
      }
      if (filter.type === 'keyword') {
        filtered = filtered.filter((event) =>
          event.title.toLowerCase().includes(filter.value.toLowerCase())
        );
      }
    });

    if (searchQuery) {
      filtered = filtered.filter((event) =>
        event.title.toLowerCase().includes(searchQuery.toLowerCase())
      );
    }

    setFilteredEvents(filtered);
  };

  const handleAddFilter = () => {
    if (!newFilterValue) return;
    setFilters([...filters, { type: newFilterType, value: newFilterValue }]);
    setNewFilterValue('');
  };

  const handleRemoveFilter = (index: number) => {
    const updatedFilters = [...filters];
    updatedFilters.splice(index, 1);
    setFilters(updatedFilters);
  };

  const handleViewAttendees = async (event: Event) => {
    try {
      const response = await fetch(
        `http://localhost:5001/api/EventAttendance/attendees/${event.id}`
      );
      if (!response.ok) throw new Error(`Error: ${response.status}`);
      const data: Attendee[] = await response.json();
      setAttendees(data);
      setPopupEvent(event);
      setViewAttendeesPopupOpen(true); // Open the popup
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
      if (!response.ok) throw new Error(`Error: ${response.status}`);
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
      if (!response.ok) throw new Error(`Error: ${response.status}`);
      setAttendedEvents(attendedEvents.filter((id) => id !== event.id));
      setConfirmationMessage(`You have successfully left "${event.title}".`);
    } catch (error) {
      console.error('Error leaving event:', error);
    }
  };

  const closeConfirmationPopup = () => setConfirmationMessage(null);
  const closeViewAttendeesPopup = () => setViewAttendeesPopupOpen(false);

  const handleClickOutside = (event: MouseEvent) => {
    if (popupRef.current && !popupRef.current.contains(event.target as Node)) {
      onClose();
    }
  };

  useEffect(() => {
    if (isOpen) {
      document.addEventListener('mousedown', handleClickOutside);
    } else {
      document.removeEventListener('mousedown', handleClickOutside);
    }
    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
    };
  }, [isOpen]);

  if (!isOpen) return null;

  return (
    <div style={styles.overlay}>
      <div style={styles.popup} ref={popupRef}>
        <h1 style={styles.header}>Search Events</h1>
        <button style={styles.closeButton} onClick={onClose}>
          Close
        </button>

        {/* Confirmation Popup */}
        {confirmationMessage && (
          <div style={styles.confirmationPopup}>
            <p>{confirmationMessage}</p>
            <button style={styles.closeButton} onClick={closeConfirmationPopup}>
              OK
            </button>
          </div>
        )}

        {/* View Attendees Popup */}
        {viewAttendeesPopupOpen && popupEvent && (
          <div style={styles.attendeesPopup}>
            <h2>Attendees for {popupEvent.title}</h2>
            {attendees.length > 0 ? (
              <ul>
                {attendees.map((attendee) => (
                  <li key={attendee.userId}>
                    {attendee.userName} (Attended at: {new Date(attendee.attendedAt).toLocaleString()})
                  </li>
                ))}
              </ul>
            ) : (
              <p>No attendees in this event yet.</p>
            )}
            <button style={styles.closeButton} onClick={closeViewAttendeesPopup}>
              Close
            </button>
          </div>
        )}


        
        {/* Accordion Section */}
        <div>
          <div
            style={styles.accordionHeader}
            onClick={() => setAccordionOpen(!accordionOpen)}
          >
            <h3>Filters</h3>
            <span>{accordionOpen ? '-' : '+'}</span>
          </div>
          {accordionOpen && (
            <div style={styles.filtersSection}>
              <h4>Active Filters</h4>
              {filters.map((filter, index) => (
                <div key={index} style={styles.filterItem}>
                  <span>
                    {filter.type}: {JSON.stringify(filter.value)}
                  </span>
                  <button onClick={() => handleRemoveFilter(index)}>Remove</button>
                </div>
              ))}
              <div style={styles.addFilter}>
                <select
                  value={newFilterType}
                  onChange={(e) => setNewFilterType(e.target.value as FilterType)}
                  style={styles.filterSelect}
                >
                  <option value="dateRange">Date Range (YYYY-MM-DD to YYYY-MM-DD)</option>
                  <option value="location">Location</option>
                  <option value="keyword">Keyword</option>
                </select>
                <input
                  type="text"
                  placeholder="Enter filter value"
                  value={newFilterValue}
                  onChange={(e) => setNewFilterValue(e.target.value)}
                  style={styles.filterInput}
                />
                <button onClick={handleAddFilter} style={styles.addFilterButton}>
                  Add
                </button>
              </div>
            </div>
          )}
        </div>

        {/* Search Bar */}
        <input
          type="text"
          placeholder="Search events by title..."
          value={searchQuery}
          onChange={(e) => setSearchQuery(e.target.value)}
          style={styles.searchBar}
        />

        {/* Event List */}
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
                  style={styles.buttonAttendees}
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
    </div>
  );
};

const styles: { [key: string]: React.CSSProperties } = {
  overlay: {
    position: 'fixed',
    top: 0,
    left: 0,
    width: '100%',
    height: '100%',
    backgroundColor: 'rgba(0, 0, 0, 0.5)',
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center',
    zIndex: 1000,
  },
  confirmationPopup: {
    position: 'fixed',
    top: '30%',
    left: '50%',
    transform: 'translate(-50%, -50%)',
    backgroundColor: '#fff',
    padding: '20px',
    borderRadius: '8px',
    boxShadow: '0 4px 6px rgba(0, 0, 0, 0.1)',
    zIndex: 1100,
    textAlign: 'center',
  },
  attendeesPopup: {
    position: 'fixed',
    top: '30%',
    left: '50%',
    transform: 'translate(-50%, -50%)',
    backgroundColor: '#fff',
    padding: '20px',
    borderRadius: '8px',
    boxShadow: '0 4px 6px rgba(0, 0, 0, 0.1)',
    zIndex: 1100,
    textAlign: 'center',
    width: '400px',
    maxHeight: '300px',
    overflowY: 'auto',
  },
  searchBar: {
    width: '100%',
    padding: '10px',
    marginTop: '16px',
    fontSize: '16px',
    border: '1px solid #ccc',
    borderRadius: '4px',
  },
  popup: {
    backgroundColor: '#fff',
    padding: '20px',
    borderRadius: '8px',
    width: '80%',
    maxWidth: '800px',
    maxHeight: '90%',
    overflowY: 'auto',
    position: 'relative',
  },
  header: {
    fontSize: '24px',
    marginBottom: '16px',
  },
  closeButton: {
    position: 'absolute',
    top: '10px',
    right: '10px',
    padding: '5px 10px',
    backgroundColor: 'red',
    color: '#fff',
    border: 'none',
    borderRadius: '4px',
    cursor: 'pointer',
  },
  accordionHeader: {
    display: 'flex',
    justifyContent: 'space-between',
    alignItems: 'center',
    backgroundColor: '#f1f1f1',
    padding: '10px',
    cursor: 'pointer',
    borderRadius: '4px',
    marginBottom: '8px',
  },
  filtersSection: {
    padding: '10px',
    backgroundColor: '#f9f9f9',
    borderRadius: '8px',
  },
  filterItem: {
    display: 'flex',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: '8px',
  },
  addFilter: {
    marginTop: '8px',
    display: 'flex',
    flexDirection: 'column',
  },
  filterSelect: {
    marginBottom: '8px',
  },
  filterInput: {
    marginBottom: '8px',
  },
  addFilterButton: {
    padding: '5px 10px',
    backgroundColor: '#007BFF',
    color: '#fff',
    border: 'none',
    borderRadius: '4px',
    cursor: 'pointer',
  },
  eventList: {
    maxHeight: '400px',
    overflowY: 'auto',
  },
  eventItem: {
    borderBottom: '1px solid #ddd',
    padding: '10px 0',
  },
  button: {
    margin: '5px',
    padding: '10px 15px',
    color: '#fff',
    border: 'none',
    borderRadius: '4px',
    cursor: 'pointer',
  },
  buttonAttendees: {
    margin: '5px',
    padding: '10px 15px',
    color: '#000',
    border: 'none',
    borderRadius: '4px',
    cursor: 'pointer',
    backgroundColor: '#ededed',
  },
};

export default SearchPopup;
