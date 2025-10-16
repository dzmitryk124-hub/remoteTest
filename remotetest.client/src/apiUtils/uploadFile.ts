export async function uploadFile(file: File): Promise<string> {
  const formData = new FormData();
  formData.append('file', file);

  const response = await fetch('meter-reading-uploads', {
    method: 'POST',
    body: formData,
  });

  if (response.ok) {
    return 'File uploaded successfully!';
  } else {
    throw new Error('File upload failed.');
  }
}