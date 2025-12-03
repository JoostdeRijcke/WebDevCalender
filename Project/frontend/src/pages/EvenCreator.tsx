import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import "../styling/EventCreator.css";

export const EventCreator: React.FC = () => {
  const [Title, setTitle] = useState("");
  const [Description, setDescription] = useState("");
  const [EventDate, setEventDate] = useState("");
  const [StartTime, setStartTime] = useState(""); // Format: HH:mm:ss
  const [EndTime, setEndTime] = useState(""); // Format: HH:mm:ss
  const [Location, setLocation] = useState("");

  const navigate = useNavigate();

  // useEffect(() => {
  //     // Check if admin is logged in
  //   const isAdminLoggedIn = localStorage.getItem("isAdminLoggedIn");
  //     if (isAdminLoggedIn !== "true") {
  //       alert("You must be logged in as an admin to access this page.");
  //       navigate("/");
  //     }
  //   }, [navigate]);
  const handleClick = () => {
    // Navigate to '/menu' when back is clicked
    navigate('/calender');
  };

  const handleCreateEvent = async () => {
    if (new Date(EventDate) < new Date()) {
      alert("Event date cannot be in the past.");
      return;
    }


    if (StartTime >= EndTime) {
      alert("End time must be after start time.");
      return;
    }
    
    if (!Title.trim()) {
      alert("Title is required.");
      return;
    }

    if (!Description.trim()) {
      alert("Description is required.");
      return;
    }

    if (!EventDate.trim()) {
      alert("Event date is required.");
      return;
    }

    if (!StartTime.trim()) {
      alert("Start time is required.");
      return;
    }

    if (!EndTime.trim()) {
      alert("End time is required.");
      return;
    }

    if (!Location.trim()) {
      alert("Location is required.");
      return;
    }


    const eventData = {
      Title,
      Description,
      Date: EventDate,
      StartTime: `${StartTime}:00`, // Add seconds manually
      EndTime: `${EndTime}:00`, // Add seconds manually
      Location,
      AdminApproval: true,
    };

    try {
      const response = await fetch("http://localhost:5001/api/Events", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(eventData),
      });

      if (response.ok) {
        alert("Event created successfully!");
        setTitle("");
        setDescription("");
        setEventDate("");
        setStartTime("");
        setEndTime("");
        setLocation("");
      } else {
        const errorData = await response.json();
        console.error("Error creating event:", errorData);
        alert(`Error: ${response.statusText}`);
      }
    } catch (error) {
      console.error("Network error:", error);
      alert("Failed to create event. Please try again.");
    }
  };

  return (
    <div className="container">
      <button className="back-button" onClick={handleClick}>
        Back
      </button>
      <h1>Create Event</h1>
      <div className="form-container">
        <input
          className="input"
          type="text"
          placeholder="Title"
          value={Title}
          onChange={(e) => setTitle(e.target.value)}
        />
        <textarea
          className="textarea"
          placeholder="Description"
          value={Description}
          onChange={(e) => setDescription(e.target.value)}
        />
        <input
          className="input"
          placeholder="Event Date"
          type="date"
          value={EventDate}
          onChange={(e) => setEventDate(e.target.value)}
        />

        
        <label className="label">Begin Time</label>
        <input
          className="input"
          type="time"
          value={StartTime}
          onChange={(e) => setStartTime(e.target.value)}
        />
        <label className="label">End Time</label>
        <input
          className="input"
          type="time"
          value={EndTime}
          onChange={(e) => setEndTime(e.target.value)}
        />
        <input
          className="input"
          type="text"
          placeholder="Location"
          value={Location}
          onChange={(e) => setLocation(e.target.value)}
        />
        <button className="create-button" onClick={handleCreateEvent}>
          Create Event
        </button>
      </div>
    </div>
  );
};
