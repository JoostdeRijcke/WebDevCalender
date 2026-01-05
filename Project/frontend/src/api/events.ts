import { apiRequest } from './client';
import { Event, Attendee } from '../types';

export interface EventCreateData {
  id: number;
  title: string;
  description: string;
  date: Date;
  startTime: string;
  endTime: string;
  location: string;
  adminApproval: boolean;
  maxAttendees?: number;
}

export interface EventUpdateData {
  title: string;
  description: string;
  date: Date;
  startTime: string;
  endTime: string;
  location: string;
  adminApproval: boolean;
  maxAttendees?: number;
}

export interface SearchParams {
  title?: string;
  location?: string;
  startDate?: Date;
  endDate?: Date;
}

export async function getEvents(onlyUpcoming = true): Promise<Event[]> {
  const url = `/Events?onlyUpcoming=${onlyUpcoming ? 'true' : 'false'}`;
  return apiRequest<Event[]>(url);
}

export async function getEventById(eventId: number): Promise<Event> {
  return apiRequest<Event>(`/Events/${eventId}`);
}

export async function getEventAttendees(eventId: number): Promise<Attendee[]> {
  return apiRequest<Attendee[]>(`/EventAttendance/attendees/${eventId}`);
}

export async function createEvent(eventData: EventCreateData): Promise<Event> {
  return apiRequest<Event>('/Events', {
    method: 'POST',
    body: eventData,
  });
}

export async function updateEvent(eventId: number, eventData: EventUpdateData): Promise<void> {
  await apiRequest(`/Events/${eventId}`, {
    method: 'PUT',
    body: eventData,
  });
}

export async function deleteEvent(eventId: number): Promise<void> {
  await apiRequest(`/Events/${eventId}`, {
    method: 'DELETE',
  });
}

export async function searchEvents(params: SearchParams): Promise<Event[]> {
  const queryParams = new URLSearchParams();

  if (params.title) queryParams.append('title', params.title);
  if (params.location) queryParams.append('location', params.location);
  if (params.startDate) queryParams.append('startDate', params.startDate.toISOString());
  if (params.endDate) queryParams.append('endDate', params.endDate.toISOString());

  return apiRequest<Event[]>(`/Events/search?${queryParams.toString()}`);
}
