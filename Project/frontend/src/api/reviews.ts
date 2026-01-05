import { apiRequest } from './client';

export interface Review {
  id: number;
  userId: number;
  eventId: number;
  rating: number;
  feedback: string;
  attendedAt: Date;
  user: {
    firstName: string;
    lastName: string;
  };
  event: {
    title: string;
    date: Date;
  };
}

export async function getAllReviews(): Promise<Review[]> {
  return apiRequest<Review[]>('/Events/reviews');
}

export async function submitReview(
  userId: number,
  eventId: number,
  rating: number,
  feedback: string
): Promise<void> {
  await apiRequest('/Events/review', {
    method: 'POST',
    body: {
      userId,
      eventId,
      rating,
      feedback,
    },
  });
}
