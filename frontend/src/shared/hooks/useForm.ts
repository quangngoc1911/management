'use client'

import { useState } from 'react'

export type Errors<T> = Partial<Record<keyof T, string>>;

export function useForm<T>(
  initialValues: T,
  validate: (values: T) => Errors<T>
) {
  const [values, setValues] = useState<T>(initialValues)
  const [errors, setErrors] = useState<Errors<T>>({})

  function handleChange(e: React.ChangeEvent<HTMLInputElement>) {
    const { name, value } = e.target
    setValues((prev) => ({ ...prev, [name]: value }))
  }

  function handleSubmit(onSubmit: (values: T) => void) {
    return (e: React.FormEvent) => {
      e.preventDefault()

      const validationErrors = validate(values)
      setErrors(validationErrors)

      if (Object.keys(validationErrors).length === 0) {
        onSubmit(values)
      }
    }
  }

  return {
    values,
    errors,
    handleChange,
    handleSubmit,
    setValues,
  }
}