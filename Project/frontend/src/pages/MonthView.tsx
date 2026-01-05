import React, { useState, useEffect } from 'react';
import '../styling/MonthView.css';
import { useNavigate } from 'react-router-dom';

type EventAttendance = {
  userId: number;
  userName: string;
  rating?: number;
  feedback?: string;
};

type Event = {
  id: number;
  title: string;
  description: string;
  date: string;
  startTime: string;
  endTime: string;
  maxAttendees?: number;
  eventAttendances: EventAttendance[] | null;
};

export const MonthView: React.FC = () => {
  const navigate = useNavigate();
  const [currentMonth, setCurrentMonth] = useState<Date>(() => new Date());
  const [events, setEvents] = useState<Event[]>([]);
  const [selectedEvent, setSelectedEvent] = useState<Event | null>(null);
  const [isAdmin, setIsAdmin] = useState(false);
  const [loggedIn, setLoggedIn] = useState(false);

  const getEventColor = (title: string): string => {
    const lowerTitle = title.toLowerCase();
    if (lowerTitle.includes('meeting') || lowerTitle.includes('standup')) return '#2196f3';
    if (lowerTitle.includes('workshop') || lowerTitle.includes('training')) return '#ff9800';
    if (lowerTitle.includes('social') || lowerTitle.includes('party') || lowerTitle.includes('lunch')) return '#e91e63';
    if (lowerTitle.includes('yoga') || lowerTitle.includes('fitness') || lowerTitle.includes('health')) return '#4caf50';
    if (lowerTitle.includes('hackathon') || lowerTitle.includes('competition')) return '#9c27b0';
    if (lowerTitle.includes('presentation') || lowerTitle.includes('demo')) return '#f44336';
    return '#607d8b';
  };

  // Get all days for the current month view (including padding from prev/next month)
  const getMonthDays = (date: Date): Date[] => {
    const year = date.getFullYear();
    const month = date.getMonth();

    // First day of the month
    const firstDay = new Date(year, month, 1);
    // Last day of the month
    const lastDay = new Date(year, month + 1, 0);

    // Get day of week for first day (0 = Sunday, 1 = Monday, etc.)
    const firstDayOfWeek = firstDay.getDay();
    // Adjust so Monday = 0
    const startOffset = firstDayOfWeek === 0 ? 6 : firstDayOfWeek - 1;

    // Calculate start date (including previous month days)
    const startDate = new Date(firstDay);
    startDate.setDate(firstDay.getDate() - startOffset);

    // Generate 42 days (6 weeks)
    const days: Date[] = [];
    for (let i = 0; i < 42; i++) {
      const day = new Date(startDate);
      day.setDate(startDate.getDate() + i);
      days.push(day);
    }

    return days;
  };

  const monthDays = getMonthDays(currentMonth);

  const getEvents = async () => {
    try {
      const response = await fetch('http://localhost:5001/api/Events');
      if (response.ok) {
        const data = await response.json();
        setEvents(data);
      } else {
        alert('Unable to load events');
        console.error(response.statusText);
      }
    } catch (error) {
      alert('Er is iets misgegaan');
      console.error('Error fetching events:', error);
    }
  };

  useEffect(() => {
    getEvents();
  }, [currentMonth]);

  const goToNextMonth = () => {
    const nextMonth = new Date(currentMonth);
    nextMonth.setMonth(currentMonth.getMonth() + 1);
    setCurrentMonth(nextMonth);
  };

  const goToPreviousMonth = () => {
    const prevMonth = new Date(currentMonth);
    prevMonth.setMonth(currentMonth.getMonth() - 1);
    setCurrentMonth(prevMonth);
  };

  const handleEventSelect = (event: Event) => {
    setSelectedEvent(event);
  };

  const checkUserLoggedIn = async () => {
    try {
      const response = await fetch('http://localhost:5001/api/IsUserLoggedIn', {
        credentials: 'include',
      });
      if (response.ok) {
        setLoggedIn(true);
      } else {
        setLoggedIn(false);
      }
    } catch (error) {
      setLoggedIn(false);
      console.error('Error checking user login status:', error);
    }
  };

  const checkAdminLoggedIn = async () => {
    try {
      const response = await fetch('http://localhost:5001/api/IsAdminLoggedIn', {
        credentials: 'include',
      });
      if (response.ok) {
        const data = await response.json();
        setIsAdmin(data.isAdminLoggedIn === true);
        setLoggedIn(true);
      } else {
        setIsAdmin(false);
      }
    } catch (error) {
      console.error('Error checking admin login status:', error);
      setIsAdmin(false);
    }
  };

  const logout = async () => {
    try {
      const response = await fetch('http://localhost:5001/api/Logout', {
        credentials: 'include',
      });
      if (response.ok) {
        setLoggedIn(false);
      } else {
        const response1 = await fetch('http://localhost:5001/api/AdminLogout', {
          credentials: 'include',
        });
        if (response1.ok) {
          setIsAdmin(false);
          setLoggedIn(false);
        }
      }
      navigate('/');
    } catch (error) {
      console.error('Error logging out:', error);
    }
  };

  useEffect(() => {
    checkAdminLoggedIn();
    checkUserLoggedIn();
  }, []);

  const isCurrentMonth = (date: Date): boolean => {
    return date.getMonth() === currentMonth.getMonth();
  };

  const isToday = (date: Date): boolean => {
    const today = new Date();
    return (
      date.getDate() === today.getDate() &&
      date.getMonth() === today.getMonth() &&
      date.getFullYear() === today.getFullYear()
    );
  };

  return (
    <div>
      {loggedIn || isAdmin ? (
        <div className="month-view-container">
          <header className="month-header">
            <button onClick={goToPreviousMonth}>&lt;=</button>
            <h2>
              {currentMonth.toLocaleDateString('en-US', {
                month: 'long',
                year: 'numeric',
              })}
            </h2>
            <button onClick={goToNextMonth}>=&gt;</button>
          </header>

          <div className="month-content">
            <div className="month-grid-container">
              {/* Day headers */}
              <div className="month-day-headers">
                {['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'].map((day) => (
                  <div key={day} className="month-day-header">
                    {day}
                  </div>
                ))}
              </div>

              {/* Calendar grid */}
              <div className="month-grid">
                {monthDays.map((date, index) => {
                  const dateStr = date.toISOString().split('T')[0];
                  const eventsForDay = events.filter((event) => event.date.split('T')[0] === dateStr);
                  const isOtherMonth = !isCurrentMonth(date);
                  const isTodayDate = isToday(date);

                  return (
                    <div
                      key={index}
                      className={`month-day ${isOtherMonth ? 'other-month' : ''} ${isTodayDate ? 'today' : ''}`}
                    >
                      <div className="month-day-number">{date.getDate()}</div>
                      <div className="month-day-events">
                        {eventsForDay.slice(0, 3).map((event) => (
                          <div
                            key={event.id}
                            className="month-event"
                            onClick={() => handleEventSelect(event)}
                            style={{ backgroundColor: getEventColor(event.title) }}
                            title={`${event.title} - ${event.startTime.split(':')[0]}:${event.startTime.split(':')[1]}`}
                          >
                            {event.title}
                          </div>
                        ))}
                        {eventsForDay.length > 3 && (
                          <div className="month-event-more">
                            +{eventsForDay.length - 3} more
                          </div>
                        )}
                      </div>
                    </div>
                  );
                })}
              </div>
            </div>

            {/* Event details sidebar */}
            <aside className="month-event-info">
              {selectedEvent ? (
                <>
                  <h3>{selectedEvent.title}</h3>
                  {isAdmin && (
                    <p style={{ color: '#888', fontSize: '0.9em' }}>Event ID: {selectedEvent.id}</p>
                  )}
                  <p>
                    <strong>Date:</strong> {selectedEvent.date.split('T')[0]} <br />
                    <strong>Time:</strong> {selectedEvent.startTime.split(':')[0]}:{selectedEvent.startTime.split(':')[1]} - {selectedEvent.endTime.split(':')[0]}:{selectedEvent.endTime.split(':')[1]}<br />
                    <strong>Attendees:</strong> {selectedEvent.eventAttendances?.length || 0}
                    {selectedEvent.maxAttendees && ` / ${selectedEvent.maxAttendees}`}
                    <br />
                    <strong>Description:</strong> {selectedEvent.description}
                  </p>
                </>
              ) : (
                <p>Select an event to see details</p>
              )}
            </aside>
          </div>

          <footer className="month-footer">
            <button onClick={() => navigate('/calender')}>Week View</button>
            <button onClick={() => navigate('/search')}>Search Events</button>
            {isAdmin && (
              <>
                <button onClick={() => navigate('/event')}>Create Event</button>
                <button onClick={() => navigate('/delete-event')}>Delete Event</button>
              </>
            )}
            <button onClick={logout}>Logout</button>
          </footer>
        </div>
      ) : (
        <h1>Please log in to use the calendar.</h1>
      )}
    </div>
  );
};
