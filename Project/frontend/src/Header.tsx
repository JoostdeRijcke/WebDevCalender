import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import SearchPopup from './SearchPopup';

const Header: React.FC = () => {
  const [role, setRole] = useState<string | null>(null);
  const [isSearchOpen, setIsSearchOpen] = useState(false);
 

  const handleSearchClick = () => {
    setIsSearchOpen(true);
  };

  const handleClosePopup = () => {
    setIsSearchOpen(false);
  };


  useEffect(() => {
    const checkRole = async () => {
      try {
        const adminResponse = await fetch(`/api/IsAdminLoggedIn?ts=${Date.now()}`, { credentials: 'include' });
        if (adminResponse.ok) {
          setRole('admin');
          return;
        }
      } catch (error) {
        console.error('Error checking admin login:', error);
      }

      try {
        const userResponse = await fetch(`/api/IsUserLoggedIn?ts=${Date.now()}`, { credentials: 'include' });
        if (userResponse.ok) {
          setRole('user');
          return;
        }
      } catch (error) {
        console.error('Error checking user login:', error);
      }

      setRole(null); // Not logged in
    };

    checkRole();
  }, []);


  return (
    <header
      style={{
        display: 'flex',
        justifyContent: 'space-between',
        alignItems: 'center',
        padding: '10px 20px',
        backgroundColor: '#f0f0f0',
        color: '#020202ff',
        borderBottom: '1px solid #ddd',
      }}
    >
      {/* Left Section */}
      <div style={{ display: 'flex', gap: '15px', flex: '1' }}>
        <Link to="/" style={{ textDecoration: 'none', color: 'black' }}>
          Calendar
        </Link>
      </div>

      {/* Middle Section */}
      <div style={{ fontSize: '24px', fontWeight: 'bold', textAlign: 'center', flex: '1' }}>
        Calendify
      </div>

      {/* Right Section */}
      <div style={{ display: 'flex', justifyContent: 'flex-end', gap: '10px', flex: '1' }}>
        <button
          onClick={handleSearchClick}
          style={{
            padding: '10px 20px',
            fontSize: '16px',
            border: '0',
            backgroundColor: '#236BE0',
            color: '#fff',
            borderRadius: '4px',
            cursor: 'pointer',
          }}
        >
          Search
        </button>
      </div>
      <SearchPopup
        isOpen={isSearchOpen}
        onClose={handleClosePopup}
        loggedInUserId={2} // Provide the required loggedInUserId prop
      />

    </header>
  );
};

export default Header;
