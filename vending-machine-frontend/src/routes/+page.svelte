<script>
	import Communicator from '$lib/VendingMachineAPI';
	import{currentUser} from '$lib/stores';
	import { goto } from '$app/navigation';
	let username,password;
	let isrespone = false;
	let message;
	async function Onlogin(){
		isrespone = false;
		var data = {
			username: username,
			password: password
		}
		if(username == null || password == null){
			message = 'username and password cannot be empty';
			isrespone = true;
			return;
		}

		const res = await Communicator.user.login(data);
		if(res.status == 200){
			const json1 = await res.json();
			//console.table(json1);
			currentUser.set({
				name : json1.username,
				role : json1.role,
				deposit : json1.deposit,
				token : json1.jwToken.token,
			})
			if(json1.role == 1)
				goto('/seller');
			else
				goto('/buyer');
			console.log(currentUser);

		}else{
			isrespone = true;
			message = await res.text();
		}
	}
</script>

<svelte:head>
	<title>Home</title>
	<meta name="description" content="Svelte demo app" />
</svelte:head>

<section class="flex flex-col justify-center items-center">
	<form on:submit|preventDefault ={Onlogin} class="card w-96 bg-base-300 shadow-xl flex gap-4 items-center py-10">
		<h2 class="menu-title text-2xl">Login</h2>
		<input bind:value={username} type="text" placeholder="Enter Username here" class="input input-bordered input-accent w-full max-w-xs" />
		<input bind:value={password} type="text" placeholder="Enter Password here" class="input input-bordered input-accent w-full max-w-xs" />
		<button class="btn btn-wide">Login</button>
		<a class="link link-primary" href="/register">Register</a>
		{#if isrespone}
		<div class="alert alert-error shadow-lg w-11/12">
			<span>{message}</span>
		</div>
		{/if}
	</form>

</section>

<style>
	section {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		flex: 0.6;
	}
</style>
