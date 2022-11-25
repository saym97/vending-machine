<script>
  import{currentUser} from '$lib/stores';
  import { onMount } from 'svelte';
  import PurchaseConfirmation from './PurchaseConfirmation.svelte';

  import BuyersCard from './BuyersCard.svelte';

	import BuyerProductCard from './BuyerProductCard.svelte';
	import Communicator from '$lib/VendingMachineAPI';
	import { goto } from '$app/navigation';
	let boughtItem = null;
  let purchaseError = null;
	let products = [
		{
			productId: '3314b86c-38a2-4457-84fe-079205b7f545',
			name: 'chocloate2',
			cost: 1100,
			amountAvailable: 3,
			sellerId: '39e2d3b5-b7e4-41be-82ba-a7ca949e39a3'
		},
		{
			productId: 'b41005be-2b7f-4fcf-88d4-35f83e763281',
			name: 'chocloate23',
			cost: 10,
			amountAvailable: 2,
			sellerId: '39e2d3b5-b7e4-41be-82ba-a7ca949e39a3'
		},
		{
			productId: '1f87837d-9fca-46bc-9651-947342bc1252',
			name: 'chocloate',
			cost: 11400,
			amountAvailable: 2,
			sellerId: '39e2d3b5-b7e4-41be-82ba-a7ca949e39a3'
		},
		{
			productId: 'c27ecde4-fe92-4f78-927c-acbf18667a3c',
			name: 'chocloate2',
			cost: 1300,
			amountAvailable: 1,
			sellerId: '39e2d3b5-b7e4-41be-82ba-a7ca949e39a3'
		},
		{
			productId: '76996f06-c2cd-453d-b5b8-b76dc554d4b2',
			name: 'KitKat2',
			cost: 15,
			amountAvailable: 1,
			sellerId: '39e2d3b5-b7e4-41be-82ba-a7ca949e39a3'
		},
		{
			productId: '00caeebf-ee74-4ddd-860b-de40915ec73b',
			name: 'KitKat',
			cost: 45,
			amountAvailable: 4,
			sellerId: '39e2d3b5-b7e4-41be-82ba-a7ca949e39a3'
		},
		{
			productId: '242bc6df-2cfc-4e23-9598-f2aed17e6cbe',
			name: 'chocloate2',
			cost: 1200,
			amountAvailable: 1,
			sellerId: '39e2d3b5-b7e4-41be-82ba-a7ca949e39a3'
		}
	];

  async function GetProducts(){
    let response = await Communicator.product.listAll();

    if (response.status == 200) {
      products = await response.json();
    }
  }
  onMount(async ()=>{
      
    if ($currentUser?.role == 1)
      goto('/');
      
    GetProducts();

  });

	async function BuyProduct(id, amount) {
    console.log("Id: " + id + " Amount: " + amount);
		let res = await Communicator.product.buy({ id, amount });

    if(res.status == 200) {
      boughtItem = await res.json();
      $currentUser.deposit = 0;
    }else{
      purchaseError = await res.text();
      setTimeout(() => {purchaseError = null},3000);
    }
	}
</script>

<BuyersCard />

{#if purchaseError}
<div class="toast">
  <div class="alert alert-info">
    <div>
      <span>{purchaseError}</span>
		</div>
	</div>
</div>
{/if}

{#if boughtItem}
<PurchaseConfirmation {...boughtItem} BuyMore={()=>{boughtItem=null; GetProducts()}} />
{:else}
<div class="w-full flex flex-wrap gap-2 p-2 my-2 ">
	{#each products as product}
		<BuyerProductCard {...product} Buy={BuyProduct} />
	{/each}
</div>
{/if}
