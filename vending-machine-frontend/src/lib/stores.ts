import { writable } from 'svelte/store';
import type { Writable } from "svelte/store";
import type { User } from './VendingMachineAPI';

export const currentUser : Writable<User|null> = writable(null);