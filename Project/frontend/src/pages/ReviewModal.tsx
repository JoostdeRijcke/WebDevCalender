import React, { useState } from 'react';
import './ReviewModal.css';

interface ReviewModalProps {
  eventId: number;
  eventTitle: string;
  userId: number;
  onClose: () => void;
  onSubmitSuccess: () => void;
}

export const ReviewModal: React.FC<ReviewModalProps> = ({
  eventId,
  eventTitle,
  userId,
  onClose,
  onSubmitSuccess,
}) => {
  const [rating, setRating] = useState<number>(0);
  const [feedback, setFeedback] = useState<string>('');
  const [hoveredRating, setHoveredRating] = useState<number>(0);
  const [isSubmitting, setIsSubmitting] = useState<boolean>(false);

  const handleSubmit = async () => {
    if (rating === 0) {
      alert('Please select a rating');
      return;
    }

    if (feedback.length > 250) {
      alert('Feedback must be 250 characters or less');
      return;
    }

    setIsSubmitting(true);

    try {
      const response = await fetch('http://localhost:5001/api/Events/review', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        credentials: 'include',
        body: JSON.stringify({
          userId: userId,
          eventId: eventId,
          rating: rating,
          feedback: feedback,
          attendedAt: new Date().toISOString(),
        }),
      });

      if (response.ok) {
        alert('Review submitted successfully!');
        onSubmitSuccess();
        onClose();
      } else {
        const errorText = await response.text();
        alert(`Failed to submit review: ${errorText}`);
      }
    } catch (error) {
      console.error('Error submitting review:', error);
      alert('An error occurred while submitting your review');
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="review-modal-overlay" onClick={onClose}>
      <div className="review-modal-content" onClick={(e) => e.stopPropagation()}>
        <h2>Review Event</h2>
        <h3>{eventTitle}</h3>

        <div className="rating-section">
          <label>Rating:</label>
          <div className="stars">
            {[1, 2, 3, 4, 5].map((star) => (
              <span
                key={star}
                className={`star ${star <= (hoveredRating || rating) ? 'filled' : ''}`}
                onClick={() => setRating(star)}
                onMouseEnter={() => setHoveredRating(star)}
                onMouseLeave={() => setHoveredRating(0)}
              >
                ★
              </span>
            ))}
          </div>
        </div>

        <div className="feedback-section">
          <label>Feedback (optional, max 250 characters):</label>
          <textarea
            value={feedback}
            onChange={(e) => setFeedback(e.target.value)}
            maxLength={250}
            placeholder="Share your thoughts about this event..."
            rows={4}
          />
          <small>{feedback.length}/250</small>
        </div>

        <div className="modal-buttons">
          <button onClick={handleSubmit} disabled={isSubmitting}>
            {isSubmitting ? 'Submitting...' : 'Submit Review'}
          </button>
          <button onClick={onClose} className="cancel-button">
            Cancel
          </button>
        </div>
      </div>
    </div>
  );
};
