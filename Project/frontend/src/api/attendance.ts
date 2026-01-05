import { apiRequest } from './client';
import { Event } from '../types';

export async function getAttendedEventsByUser(userId: number): Promise<Event[]> {
  return apiRequest<Event[]>(`/EventAttendance/user/${userId}/attended-events`);
}

export async function attendEvent(userId: number, eventId: number): Promise<void> {
  await apiRequest('/EventAttendance/attend', {
    method: 'POST',
    body: { userId, eventId },
  });
}

export async function leaveEvent(userId: number, eventId: number): Promise<void> {
  await apiRequest('/EventAttendance/remove', {
    method: 'DELETE',
    body: { userId, eventId },
  });
}
