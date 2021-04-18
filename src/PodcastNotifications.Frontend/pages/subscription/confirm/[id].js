import { useEffect, useState } from 'react'
import Head from 'next/head'
import { useRouter } from 'next/router'
import Nav from '../../../components/nav'

export default function ConfirmSubscription() {

  const router = useRouter()
  const { id } = router.query
  const [success, setSuccess] = useState(null)

  useEffect(() => {
    id && confirm()
  }, [id])

  const confirm = async () => {

    const body = {
      subscriptionToken: id
    }
    const json = JSON.stringify(body)
    const response = await fetch(`${process.env.BACKEND_URL}confirm-subscription`, {
      method: 'POST',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      },
      body: json
    });
    if (response.status === 200) {
      setSuccess(true)
    } else {
      setSuccess(false)
    }
  }

  return (
    <div>
      <Head>
        <title>Confirm Subscription | Podcast Notifications</title>
        <link rel="icon" href="/favicon.ico" />
        <link rel="stylesheet" href="https://rsms.me/inter/inter.css"></link>
      </Head>
      <Nav />
      <div className="mt-10 max-w-full mx-auto px-4 sm:px-6 lg:px-8">
        <div className="max-w-5xl mx-auto text-center">
          {success != null && success && <>
            <h1 className="text-4xl tracking-tight font-extrabold text-gray-900 sm:text-5xl md:text-6xl">
              Subscription confirmed
          </h1>
            <p className="mt-3 max-w-md mx-auto text-base text-gray-500 sm:text-lg md:mt-5 md:text-xl md:max-w-3xl">
              We've sent you an email confirming your subscription. You will now receive an email each time an episode is published.
            </p>
            <p className="mt-3 max-w-md mx-auto text-base text-gray-500 sm:text-lg md:mt-5 md:text-xl md:max-w-3xl">
              You can unsubscribe at any time using the link in the email.
            </p>
          </>
          }
          {success != null && !success && <>
            <h1 className="text-4xl tracking-tight font-extrabold text-gray-900 sm:text-5xl md:text-6xl">
              There was a problem
            </h1>
            <p className="mt-3 max-w-md mx-auto text-base text-gray-500 sm:text-lg md:mt-5 md:text-xl md:max-w-3xl">
              There was a problem confirming your subscription.
            </p>
            <p className="mt-3 max-w-md mx-auto text-base text-gray-500 sm:text-lg md:mt-5 md:text-xl md:max-w-3xl">
              This could be because you've already confirmed your subscription or the link you are using isn't valid.
            </p>
          </>
          }
        </div>
      </div>
    </div>
  )
}