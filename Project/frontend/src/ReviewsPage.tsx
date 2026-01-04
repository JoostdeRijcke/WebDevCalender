import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import './ReviewsPage.css';

interface Review {
  id: number;
  userId: number;
  eventId: number;
  rating: number;
  feedback: string;
  attendedAt: string;
  user: {
    firstName: string;
    lastName: string;
  };
  event: {
    title: string;
    date: string;
  };
}

export const ReviewsPage: React.FC = () => {
  const [reviews, setReviews] = useState<Review[]>([]);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    fetchReviews();
  }, []);

  const fetchReviews = async () => {
    try {
      const response = await fetch('http://localhost:5001/api/Events/reviews', {
        credentials: 'include',
      });

      if (response.ok) {
        const data = await response.json();
        setReviews(data);
      } else {
        console.error('Failed to fetch reviews');
      }
    } catch (error) {
      console.error('Error fetching reviews:', error);
    } finally {
      setLoading(false);
    }
  };

  const renderStars = (rating: number) => {
    return '★'.repeat(rating) + '☆'.repeat(5 - rating);
  };

  const getAverageRating = () => {
    if (reviews.length === 0) return 0;
    const total = reviews.reduce((sum, review) => sum + review.rating, 0);
    return (total / reviews.length).toFixed(1);
  };

  const getRatingDistribution = () => {
    const distribution = [0, 0, 0, 0, 0];
    reviews.forEach(review => {
      if (review.rating >= 1 && review.rating <= 5) {
        distribution[review.rating - 1]++;
      }
    });
    return distribution.reverse();
  };

  if (loading) {
    return (
      <div className="reviews-container">
        <p>Loading reviews...</p>
      </div>
    );
  }

  const ratingDistribution = getRatingDistribution();

  return (
    <div className="reviews-container">
      <button className="back-button" onClick={() => navigate('/calender')}>
        Back to Calendar
      </button>

      <h1>Event Reviews</h1>

      <div className="reviews-stats">
        <div className="average-rating">
          <h2>{getAverageRating()}</h2>
          <div className="stars-large">{renderStars(Math.round(Number(getAverageRating())))}</div>
          <p>{reviews.length} reviews</p>
        </div>

        <div className="rating-bars">
          {[5, 4, 3, 2, 1].map((star, index) => {
            const count = ratingDistribution[index];
            const percentage = reviews.length > 0 ? (count / reviews.length) * 100 : 0;
            return (
              <div key={star} className="rating-bar-row">
                <span className="star-label">{star} ★</span>
                <div className="rating-bar">
                  <div className="rating-bar-fill" style={{ width: `${percentage}%` }}></div>
                </div>
                <span className="rating-count">{count}</span>
              </div>
            );
          })}
        </div>
      </div>

      <div className="reviews-list">
        {reviews.length === 0 ? (
          <p className="no-reviews">No reviews yet</p>
        ) : (
          reviews.map((review) => (
            <div key={review.id} className="review-card">
              <div className="review-header">
                <div>
                  <h3>{review.event.title}</h3>
                  <p className="reviewer-name">
                    {review.user.firstName} {review.user.lastName}
                  </p>
                </div>
                <div className="review-rating">
                  <span className="stars">{renderStars(review.rating)}</span>
                  <span className="rating-number">{review.rating}/5</span>
                </div>
              </div>
              {review.feedback && (
                <p className="review-feedback">{review.feedback}</p>
              )}
              <p className="review-date">
                Attended: {new Date(review.attendedAt).toLocaleDateString()}
              </p>
            </div>
          ))
        )}
      </div>
    </div>
  );
};
