import React, { useState, useEffect } from 'react';
import '../styling/CalenderPage.css';
import { useNavigate } from 'react-router-dom';
import { ReviewModal } from './ReviewModal';

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


export const CalendarPage: React.FC = () => {

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

  const [currentWeekStart, setCurrentWeekStart] = useState<Date>(() => {
    const today = new Date();
    const startOfWeek = new Date(today);
    startOfWeek.setDate(today.getDate() - today.getDay() + 1);
    return startOfWeek;
  });

  const getWeekDates = (start: Date): Date[] => {
    return Array.from({ length: 7 }, (_, index) => {
      const date = new Date(start);
      date.setDate(start.getDate() + index);
      return date;
    });
  };


  const weekDates = getWeekDates(currentWeekStart);
  const [selectedEvent, setSelectedEvent] = useState<Event | null>(null);
  const [events, setEvents] = useState<Event[]>([]);
  const navigate = useNavigate();



  const getEvents = async () => {
    try {
      const response = await fetch('http://localhost:5001/api/Events');

      if (response.ok) {
        const data = await response.json();
        setEvents(data);
      } else {
        alert('Unable to load events')
        console.error(response.statusText)
      }

    } catch (error) {
      alert("Er is iets misgegaan");
      console.error('Error fetching events:', error);
    }

  };

  useEffect(() => {
    getEvents();
  }, [currentWeekStart]);


  const goToNextWeek = () => {
    const nextWeekStart = new Date(currentWeekStart);
    nextWeekStart.setDate(currentWeekStart.getDate() + 7);
    setCurrentWeekStart(nextWeekStart);
  };

  const goToPreviousWeek = () => {
    const prevWeekStart = new Date(currentWeekStart);
    prevWeekStart.setDate(currentWeekStart.getDate() - 7);
    setCurrentWeekStart(prevWeekStart);
  };

  const handleEventSelect = (event: Event) => {
    setSelectedEvent(event);
  };

  const handleCreateEvent = () => {
    console.log('Create Event clicked');
    navigate('/event');
  };

  const handleDeleteEvent = () => {//implement this
    console.log('Delete Event clicked');
    navigate('/delete-event');
  };

  const handleRegister = () => {
    console.log('Register clicked');
    navigate('/registerpopup');

  };

  const handleAttendEvent = async () => {
    if (!selectedEvent) {
      alert('Please select an event');
      return;
    }

    if (!loggedIn && !isAdmin) {
      alert('Please login to attend events');
      return;
    }

    // For admins, we still try to make the request so backend can return proper error
    // For regular users, currentUserId should be set
    if (!isAdmin && !currentUserId) {
      alert('Please login to attend events');
      return;
    }

    try {
      const response = await fetch('http://localhost:5001/api/EventAttendance/attend', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        credentials: 'include',
        body: JSON.stringify({
          userId: currentUserId || 0, // Send 0 for admins (backend will reject anyway)
          eventId: selectedEvent.id,
        }),
      });

      if (response.ok) {
        alert(`Successfully registered for ${selectedEvent.title}!`);
        // Refresh events to update attendance count
        await getEvents();
        // Fetch the updated event details to refresh the selected event
        const updatedEventResponse = await fetch(`http://localhost:5001/api/Events/${selectedEvent.id}`);
        if (updatedEventResponse.ok) {
          const updatedEvent = await updatedEventResponse.json();
          setSelectedEvent(updatedEvent);
        }
      } else {
        const errorText = await response.text();
        alert(`Failed to register: ${errorText}`);
      }
    } catch (error) {
      console.error('Error attending event:', error);
      alert('An error occurred while registering for the event');
    }
  };

  const [isAdmin, setIsAdmin] = useState(false);
  const [loggedIn, setLoggedIn] = useState(false);
  const [currentUserId, setCurrentUserId] = useState<number | null>(null);
  const [showReviewModal, setShowReviewModal] = useState(false);

  const getCurrentUser = async () => {
    try {
      const response = await fetch('http://localhost:5001/api/GetCurrentUser', {
        credentials: 'include',
      });
      if (response.ok) {
        const userData = await response.json();
        setCurrentUserId(userData.id);
        console.log('Current user ID:', userData.id);
      }
    } catch (error) {
      console.error('Error fetching current user:', error);
    }
  };

  const checkUserLoggedIn = async () => {
    try {
      const response = await fetch('http://localhost:5001/api/IsUserLoggedIn', {
        credentials: 'include',
      });
      if (response.ok) {
        setLoggedIn(true);
        console.log('User is logged in.');
        await getCurrentUser();
      } else {
        setLoggedIn(false);
        console.log('User is not logged in.');
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
        setIsAdmin(true);
        setLoggedIn(true);
        console.log('Admin is logged in.');
      } else {
        console.log('Admin is not logged in.');
        setIsAdmin(false);
      }
    } catch (error) {
      console.error('Error checking admin login status:', error);
      setIsAdmin(false)
    }
  };

  const logout = async () => {
    try {
      const response = await fetch('http://localhost:5001/api/Logout', {
        credentials: 'include',
      });
      if (response.ok) {
        console.log('Logged out successfully.');
        setLoggedIn(false);
      } else {
        const response1 = await fetch('http://localhost:5001/api/AdminLogout', {
          credentials: 'include',
        });
        if (response1.ok) {
          console.log('Logged out successfully.');
          setIsAdmin(false);
          setLoggedIn(false);
        } else {
          console.log('Error during logout.');
        }
        navigate('/');
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

  return (
    <div>
      {loggedIn || isAdmin ? <div>
        <div className="calendar-page">
          <header className="calendar-header">
            <button onClick={goToPreviousWeek}>&lt;=</button>
            <h2>
              {weekDates[0].toLocaleDateString('en-US', {
                day: 'numeric',
                month: 'short',
              })}{' '}
              -{' '}
              {weekDates[6].toLocaleDateString('en-US', {
                day: 'numeric',
                month: 'short',
                year: 'numeric',
              })}
            </h2>
            <button onClick={goToNextWeek}>=&gt;</button>
          </header>

          <main className="calendar-grid">
            <div className="calendar-days">
              {weekDates.map((date, index) => {
                const dateStr = date.toISOString().split('T')[0];
                const eventsForDay = events.filter((event) => event.date.split('T')[0] === dateStr);

                return (
                  <div key={index} className="calendar-day">
                    <h3>
                      {date.toLocaleDateString('en-US', {
                        weekday: 'short',
                        day: 'numeric',
                      })}
                    </h3>
                    <div className="calendar-events">
                      {eventsForDay.length > 0 ? (
                        eventsForDay.map((event) => (
                          <div
                            key={event.id}
                            className="calendar-event"
                            onClick={() => handleEventSelect(event)}
                            style={{ borderLeft: `4px solid ${getEventColor(event.title)}` }}
                          >
                            {event.title} - {event.startTime.split(":")[0] + ":" + event.startTime.split(":")[1]}
                          </div>
                        ))
                      ) : (
                        <p>No events</p>
                      )}
                    </div>
                  </div>
                );
              })}
            </div>

            <aside className="event-info">
              {selectedEvent ? (
                <>
                  <h3 style={{ marginBottom: '5px' }}>{selectedEvent.title}</h3>
                  {isAdmin && (
                    <p style={{ color: '#888', fontSize: '0.9em', margin: '5px 0 15px 0' }}>Event ID: {selectedEvent.id}</p>
                  )}
                  {(() => {
                    const reviews = selectedEvent.eventAttendances?.filter(a => a.rating) || [];
                    const avgRating = reviews.length > 0
                      ? (reviews.reduce((sum, a) => sum + (a.rating || 0), 0) / reviews.length).toFixed(1)
                      : 'No ratings yet';
                    return (
                      <div className="rating-display">
                        {avgRating !== 'No ratings yet' && (
                          <span className="stars-display">
                            {'★'.repeat(Math.round(Number(avgRating)))}{'☆'.repeat(5 - Math.round(Number(avgRating)))}
                          </span>
                        )}
                        <span className="rating-text">{avgRating} {reviews.length > 0 && `(${reviews.length} reviews)`}</span>
                      </div>
                    );
                  })()}
                  <p>
                    Attendees: {selectedEvent.eventAttendances?.length || 0}
                    {selectedEvent.maxAttendees && ` / ${selectedEvent.maxAttendees}`}
                    {selectedEvent.maxAttendees && (selectedEvent.eventAttendances?.length || 0) >= selectedEvent.maxAttendees && (
                      <span className="event-full-badge"> (FULL)</span>
                    )}
                    <br />
                    Date: {selectedEvent.date.split("T")[0]} <br />
                    Time: {selectedEvent.startTime.split(":")[0] + ":" + selectedEvent.startTime.split(":")[1]} - {selectedEvent.endTime.split(":")[0] + ":" + selectedEvent.endTime.split(":")[1]}<br />
                    Description: {selectedEvent.description}
                  </p>
                  <div className="event-actions">
                    {!isAdmin && (
                      <button
                        onClick={handleAttendEvent}
                        disabled={selectedEvent.maxAttendees ? (selectedEvent.eventAttendances?.length || 0) >= selectedEvent.maxAttendees : false}
                      >
                        {selectedEvent.maxAttendees && (selectedEvent.eventAttendances?.length || 0) >= selectedEvent.maxAttendees ? 'Event Full' : 'Attend Event'}
                      </button>
                    )}
                    {isAdmin && (
                      <p style={{ color: '#666', fontStyle: 'italic' }}>Admins can't attend events</p>
                    )}
                    {isAdmin ? (
                      <button onClick={() => navigate('/reviews')} className="review-button">Check All Reviews</button>
                    ) : (
                      <button onClick={() => setShowReviewModal(true)} className="review-button">Leave Review</button>
                    )}
                  </div>
                  {selectedEvent.eventAttendances && selectedEvent.eventAttendances.some(a => a.rating) && (
                    <div className="reviews-list">
                      <h4>Reviews:</h4>
                      {selectedEvent.eventAttendances
                        .filter(a => a.rating)
                        .map((attendance, idx) => (
                          <div key={idx} className="review-item">
                            <div className="review-header">
                              <strong>{attendance.userName}</strong>
                              <span className="review-stars">{'★'.repeat(attendance.rating || 0)}</span>
                            </div>
                            {attendance.feedback && <p className="review-feedback">{attendance.feedback}</p>}
                          </div>
                        ))}
                    </div>
                  )}
                </>
              ) : (
                <p>No event selected</p>
              )}
            </aside>
          </main>

          <footer>
            <button className="month-view-button" onClick={() => navigate('/month')}>
              Month View
            </button>

            {isAdmin ? <div>
              <button className="create-event-button" onClick={handleCreateEvent}>
                Create Event
              </button>
              <button className="delete-event-button" onClick={handleDeleteEvent}>
                Delete Event
              </button>
              <button className="register-button" onClick={handleRegister}>
                Register new user
              </button></div> : <div></div>}

            <button className="view-reviews-button" onClick={() => navigate('/reviews')}>
              View All Reviews
            </button>

            <button className="logout-button" onClick={logout}>
              Logout
            </button>

          </footer>
        </div>
      </div> :
        <h1>Please log in to use the calendar.</h1>}

      {showReviewModal && selectedEvent && currentUserId && (
        <ReviewModal
          eventId={selectedEvent.id}
          eventTitle={selectedEvent.title}
          userId={currentUserId}
          onClose={() => setShowReviewModal(false)}
          onSubmitSuccess={() => getEvents()}
        />
      )}
    </div>
  );
};
