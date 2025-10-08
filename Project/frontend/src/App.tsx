import React, { useState, useEffect } from 'react';
import ReactDOM from 'react-dom/client';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Header from './Header'; // Import the Header component
import { UserInput } from './LoginPage';
import { EventCreator } from './EvenCreator';
import { CalendarPage } from './CalenderPage';
import { AlterEvent } from './AlterEvent';
import { RegisterPopup } from './ResgisterPopup';
import { DeleteEvent } from './DeletePopup';
import { ForgotPassword } from './ForgotPassword';

import SearchPage from './SearchPage';
import './Styles.css';

const App: React.FC = () => {
  return (
    <Router>
      <div>
        <Header />

        <Routes>
          <Route path="/" element={<UserInput />} />
          <Route path="/calender" element={<CalendarPage />} />
          <Route path="/event" element={<EventCreator />} />
          <Route path="/search" element={<SearchPage />} />
          <Route path="/alter" element={<AlterEvent />} />
          <Route path="/delete-event" element={<DeleteEvent />} />
          <Route path="/registerpopup" element={<RegisterPopup />} />
          <Route path="/forgot" element={<ForgotPassword />} />
        </Routes>
      </div>
    </Router>
  );
};

// Initialize the app only once
const container = document.getElementById('root') as HTMLElement;
const root = ReactDOM.createRoot(container);

// Use root.render() to render the app
root.render(
  <React.StrictMode>
    <App />
  </React.StrictMode>
);

export default App;