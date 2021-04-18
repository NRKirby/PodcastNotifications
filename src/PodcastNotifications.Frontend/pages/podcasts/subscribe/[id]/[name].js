import { Fragment, useState, useEffect } from 'react'
import Head from 'next/head'
import Nav from '../../../../components/nav'
import { Dialog, Transition } from '@headlessui/react'
import { XIcon } from '@heroicons/react/outline'
import { useRouter } from 'next/router'

export default function Submit() {

    const router = useRouter()
    const [open, setOpen] = useState(true)
    const {id, name} = router.query
    const [emailInput, setEmailInput] = useState({})
    const [error, setError] = useState('')

    const handleInputChange = (e) => {
        setEmailInput({ ...emailInput, [e.currentTarget.name]: e.currentTarget.value })
    }

    useEffect(() => {
        document.getElementById('email').focus()
    }, [id])

    const submitEmail = async () => {
        const body = {
            subscriptionId: id,
            emailAddress: emailInput.email
        }
        const json = JSON.stringify(body)
        const response = await fetch(`${process.env.BACKEND_URL}create-subscription`, {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: json
        });
        if (response.status !== 200) {
            setError(`${await response.json()} Please try again.`);
        } else {
            router.push('/podcasts');
        }
    }

    const close = () => {
        setOpen(false);
        router.push('/podcasts')
    }

    return (<>
        <Head>
            <title>Subscribe | Podcast Notifications</title>
            <link rel="icon" href="/favicon.ico" />
            <link rel="stylesheet" href="https://rsms.me/inter/inter.css"></link>
        </Head>
        <Nav />
        <Transition.Root show={open} as={Fragment}>
            <Dialog as="div" static className="fixed z-10 inset-0 overflow-y-auto" open={open} onClose={close}>
                <div className="flex items-end justify-center min-h-screen pt-4 px-4 pb-20 text-center sm:block sm:p-0">
                    <Transition.Child
                        as={Fragment}
                        enter="ease-out duration-300"
                        enterFrom="opacity-0"
                        enterTo="opacity-100"
                        leave="ease-in duration-200"
                        leaveFrom="opacity-100"
                        leaveTo="opacity-0"
                    >
                        <Dialog.Overlay className="fixed inset-0 bg-gray-500 bg-opacity-75 transition-opacity" />
                    </Transition.Child>
                    <span className="hidden sm:inline-block sm:align-middle sm:h-screen" aria-hidden="true">
                        &#8203;
                    </span>
                    <Transition.Child
                        as={Fragment}
                        enter="ease-out duration-300"
                        enterFrom="opacity-0 translate-y-4 sm:translate-y-0 sm:scale-95"
                        enterTo="opacity-100 translate-y-0 sm:scale-100"
                        leave="ease-in duration-200"
                        leaveFrom="opacity-100 translate-y-0 sm:scale-100"
                        leaveTo="opacity-0 translate-y-4 sm:translate-y-0 sm:scale-95"
                    >
                        <div className="inline-block align-bottom bg-white rounded-lg px-4 pt-5 pb-4 text-left overflow-hidden shadow-xl transform transition-all sm:my-8 sm:align-middle sm:max-w-lg sm:w-full sm:p-6">
                            <div className="hidden sm:block absolute top-0 right-0 pt-4 pr-4">
                                <button
                                    type="button"
                                    className="bg-white rounded-md text-gray-400 hover:text-gray-500 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500"
                                    onClick={() => close()}
                                >
                                    <span className="sr-only">Close</span>
                                    <XIcon className="h-6 w-6" aria-hidden="true" />
                                </button>
                            </div>
                            <div className="sm:flex sm:items-start">
                                <div className="text-center sm:mt-0 sm:text-left">
                                    <Dialog.Title as="h3" className="text-lg leading-6 font-medium text-gray-900">
                                        Subscribe to {name} {name && name.toLowerCase().includes('podcast') ? '' : 'Podcast'}
                                    </Dialog.Title>
                                </div>
                            </div>
                            <div className="mt-2">
                                <p className="text-sm text-gray-500">
                                    Enter your email below to subscribe for email notifications. You can unsubscribe at any time.
                                </p>
                            </div>
                            <div className="mt-5 mb-5">
                                <input type="text" name="email" id="email" className="shadow-sm focus:ring-blue-500 focus:border-blue-500 block w-full sm:text-sm border-gray-300 rounded-md" placeholder="Enter your email address" onFocus={() => setError('')} onChange={handleInputChange} />
                            </div>
                            {error &&
                                <p className="mt-5 text-sm text-red-400">
                                    {error}
                                </p>
                            }
                            <div className="mt-5 sm:mt-4 sm:flex sm:flex-row-reverse">
                                <button
                                    type="button"
                                    className="w-full inline-flex justify-center rounded-md border border-transparent shadow-sm px-4 py-2 bg-blue-600 text-base font-medium text-white hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 sm:ml-3 sm:w-auto sm:text-sm"
                                    onClick={() => submitEmail()}
                                >
                                    Submit
                                </button>
                            </div>
                        </div>
                    </Transition.Child>
                </div>
            </Dialog>
        </Transition.Root>
    </>)
}
