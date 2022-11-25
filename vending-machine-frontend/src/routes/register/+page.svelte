<script>
	import Communicator from '$lib/VendingMachineAPI';

	let username,password,role;
	let isrespone = false;
	let message;
	async function OnRegister(){
		isrespone = false;
		if(username == null || password == null){
			message = 'username and password cannot be empty';
			isrespone = true;
			return;
		}

		const res = await Communicator.user.register({username, password,role});
		if(res.status == 200){
			const json1 = await res.json();
			console.table(json1);
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
	<form on:submit|preventDefault ={OnRegister} class="card w-96 bg-base-300 shadow-xl flex gap-4 items-center py-10">
		<h2 class="menu-title text-2xl">Register</h2>
		<input bind:value={username} type="text" placeholder="Enter Username here" class="input input-bordered input-accent w-full max-w-xs" />
		<input bind:value={password} type="text" placeholder="Enter Password here" class="input input-bordered input-accent w-full max-w-xs" />
        <select bind:value={role} class="select select-accent w-full max-w-xs">
            <option disabled selected>Choose a role</option>
            <option value = 0 >Buyer</option>
            <option value = 1 >Seller</option>
        </select>
		<button class="btn btn-wide">Register</button>
		<a class="link link-primary" href='/'>Login</a>
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
